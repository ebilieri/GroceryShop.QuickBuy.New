using FluentAssertions;
using GroceryShop.Dominio.Entidades;
using Xunit;

namespace GroceryShop.Tests.Dominio
{
    public class UsuarioTests
    {
        [Fact]
        public void Usuario_DeveInicializarPropriedadesCorretamente()
        {
            // Arrange & Act
            var usuario = new Usuario
            {
                Id = 1,
                Email = "joao@teste.com",
                Senha = "senha123",
                Nome = "João",
                SobreNome = "Silva"
            };

            // Assert
            usuario.Id.Should().Be(1);
            usuario.Email.Should().Be("joao@teste.com");
            usuario.Senha.Should().Be("senha123");
            usuario.Nome.Should().Be("João");
            usuario.SobreNome.Should().Be("Silva");
        }

        [Fact]
        public void Usuario_DevePermitirColecaoDePedidos()
        {
            // Arrange & Act
            var usuario = new Usuario
            {
                Email = "maria@teste.com",
                Senha = "senha",
                Nome = "Maria",
                SobreNome = "Santos",
                Pedidos = new List<Pedido>()
            };

            // Assert
            usuario.Pedidos.Should().NotBeNull();
            usuario.Pedidos.Should().BeEmpty();
        }

        [Fact]
        public void Usuario_DeveAdicionarPedidosNaColecao()
        {
            // Arrange
            var usuario = new Usuario
            {
                Email = "pedro@teste.com",
                Senha = "senha",
                Nome = "Pedro",
                SobreNome = "Oliveira",
                Pedidos = new List<Pedido>()
            };

            var pedido = new Pedido
            {
                DataPedido = DateTime.Now,
                UsuarioId = usuario.Id,
                FormaPagamentoId = 1
            };

            // Act
            usuario.Pedidos.Add(pedido);

            // Assert
            usuario.Pedidos.Should().HaveCount(1);
            usuario.Pedidos.First().Should().Be(pedido);
        }

        [Fact]
        public void Usuario_DeveTerIdHerdadoDaEntidadeBase()
        {
            // Arrange & Act
            var usuario = new Usuario { Id = 456 };

            // Assert
            usuario.Should().BeAssignableTo<Entidade>();
            usuario.Id.Should().Be(456);
        }
    }
}
