using FluentAssertions;
using GroceryShop.Angular.Controllers;
using GroceryShop.Dominio.Contratos;
using GroceryShop.Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GroceryShop.Tests.Controllers
{
    public class PedidoControllerTests
    {
        private readonly Mock<IPedidoRepositorio> _mockRepositorio;
        private readonly PedidoController _controller;

        public PedidoControllerTests()
        {
            _mockRepositorio = new Mock<IPedidoRepositorio>();
            _controller = new PedidoController(_mockRepositorio.Object);
        }

        [Fact]
        public void Post_ComPedidoValido_DeveRetornarOk()
        {
            // Arrange
            var pedido = new Pedido
            {
                DataPedido = DateTime.Now,
                UsuarioId = 1,
                FormaPagamentoId = 1,
                ItensPedidos = new List<ItemPedido>
                {
                    new ItemPedido { ProdutoId = 1, Quantidade = 2 }
                }
            };

            _mockRepositorio.Setup(r => r.Adicionar(It.IsAny<Pedido>()));

            // Act
            var result = _controller.Post(pedido);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            
            _mockRepositorio.Verify(r => r.Adicionar(pedido), Times.Once);
        }

        [Fact]
        public void Post_QuandoOcorreErro_DeveRetornarBadRequest()
        {
            // Arrange
            var pedido = new Pedido
            {
                DataPedido = DateTime.Now,
                UsuarioId = 1,
                FormaPagamentoId = 1
            };
            
            _mockRepositorio.Setup(r => r.Adicionar(It.IsAny<Pedido>()))
                .Throws(new Exception("Erro ao salvar pedido"));

            // Act
            var result = _controller.Post(pedido);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
        }

        [Fact]
        public void Post_ComPedidoCompleto_DeveProcessarItensPedidos()
        {
            // Arrange
            var pedido = new Pedido
            {
                DataPedido = DateTime.Now,
                UsuarioId = 1,
                FormaPagamentoId = 1,
                ItensPedidos = new List<ItemPedido>
                {
                    new ItemPedido { ProdutoId = 1, Quantidade = 2 },
                    new ItemPedido { ProdutoId = 2, Quantidade = 3 }
                }
            };

            _mockRepositorio.Setup(r => r.Adicionar(It.IsAny<Pedido>()));

            // Act
            var result = _controller.Post(pedido);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            pedido.ItensPedidos.Should().HaveCount(2);
            _mockRepositorio.Verify(r => r.Adicionar(It.Is<Pedido>(
                p => p.ItensPedidos.Count == 2
            )), Times.Once);
        }
    }
}
