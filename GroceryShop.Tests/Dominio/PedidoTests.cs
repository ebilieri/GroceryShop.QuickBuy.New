using FluentAssertions;
using GroceryShop.Dominio.Entidades;
using Xunit;

namespace GroceryShop.Tests.Dominio
{
    public class PedidoTests
    {
        [Fact]
        public void Pedido_DeveInicializarPropriedadesCorretamente()
        {
            // Arrange
            var dataPedido = DateTime.Now;

            // Act
            var pedido = new Pedido
            {
                Id = 1,
                DataPedido = dataPedido,
                UsuarioId = 10,
                FormaPagamentoId = 2
            };

            // Assert
            pedido.Id.Should().Be(1);
            pedido.DataPedido.Should().Be(dataPedido);
            pedido.UsuarioId.Should().Be(10);
            pedido.FormaPagamentoId.Should().Be(2);
        }

        [Fact]
        public void Pedido_DevePermitirColecaoDeItensPedidos()
        {
            // Arrange & Act
            var pedido = new Pedido
            {
                DataPedido = DateTime.Now,
                UsuarioId = 1,
                FormaPagamentoId = 1,
                ItensPedidos = new List<ItemPedido>()
            };

            // Assert
            pedido.ItensPedidos.Should().NotBeNull();
            pedido.ItensPedidos.Should().BeEmpty();
        }

        [Fact]
        public void Pedido_DeveAdicionarItensPedidosNaColecao()
        {
            // Arrange
            var pedido = new Pedido
            {
                DataPedido = DateTime.Now,
                UsuarioId = 1,
                FormaPagamentoId = 1,
                ItensPedidos = new List<ItemPedido>()
            };

            var item1 = new ItemPedido { ProdutoId = 1, Quantidade = 2 };
            var item2 = new ItemPedido { ProdutoId = 2, Quantidade = 3 };

            // Act
            pedido.ItensPedidos.Add(item1);
            pedido.ItensPedidos.Add(item2);

            // Assert
            pedido.ItensPedidos.Should().HaveCount(2);
            pedido.ItensPedidos.Should().Contain(item1);
            pedido.ItensPedidos.Should().Contain(item2);
        }

        [Fact]
        public void Pedido_DeveRelacionarComUsuario()
        {
            // Arrange
            var usuario = new Usuario
            {
                Id = 1,
                Email = "teste@teste.com",
                Nome = "Teste",
                SobreNome = "Usuario"
            };

            // Act
            var pedido = new Pedido
            {
                DataPedido = DateTime.Now,
                UsuarioId = usuario.Id,
                Usuario = usuario,
                FormaPagamentoId = 1
            };

            // Assert
            pedido.UsuarioId.Should().Be(usuario.Id);
            pedido.Usuario.Should().Be(usuario);
        }

        [Fact]
        public void Pedido_DeveTerIdHerdadoDaEntidadeBase()
        {
            // Arrange & Act
            var pedido = new Pedido { Id = 789 };

            // Assert
            pedido.Should().BeAssignableTo<Entidade>();
            pedido.Id.Should().Be(789);
        }
    }
}
