using FluentAssertions;
using GroceryShop.Dominio.Entidades;
using Xunit;

namespace GroceryShop.Tests.Dominio
{
    public class ItemPedidoTests
    {
        [Fact]
        public void ItemPedido_DeveInicializarPropriedadesCorretamente()
        {
            // Arrange & Act
            var itemPedido = new ItemPedido
            {
                Id = 1,
                ProdutoId = 10,
                Quantidade = 5,
                PedidoId = 20
            };

            // Assert
            itemPedido.Id.Should().Be(1);
            itemPedido.ProdutoId.Should().Be(10);
            itemPedido.Quantidade.Should().Be(5);
            itemPedido.PedidoId.Should().Be(20);
        }

        [Fact]
        public void ItemPedido_DeveRelacionarComPedido()
        {
            // Arrange
            var pedido = new Pedido
            {
                Id = 1,
                DataPedido = DateTime.Now,
                UsuarioId = 1,
                FormaPagamentoId = 1
            };

            // Act
            var itemPedido = new ItemPedido
            {
                PedidoId = pedido.Id,
                ProdutoId = 1,
                Quantidade = 3
            };

            // Assert
            itemPedido.PedidoId.Should().Be(pedido.Id);
        }

        [Fact]
        public void ItemPedido_DevePermitirQuantidadeUm()
        {
            // Arrange & Act
            var itemPedido = new ItemPedido
            {
                ProdutoId = 1,
                Quantidade = 1
            };

            // Assert
            itemPedido.Quantidade.Should().Be(1);
        }

        [Fact]
        public void ItemPedido_DevePermitirQuantidadeMultipla()
        {
            // Arrange & Act
            var itemPedido = new ItemPedido
            {
                ProdutoId = 1,
                Quantidade = 100
            };

            // Assert
            itemPedido.Quantidade.Should().Be(100);
        }

        [Fact]
        public void ItemPedido_DeveTerIdHerdadoDaEntidadeBase()
        {
            // Arrange & Act
            var itemPedido = new ItemPedido { Id = 999 };

            // Assert
            itemPedido.Should().BeAssignableTo<Entidade>();
            itemPedido.Id.Should().Be(999);
        }
    }
}
