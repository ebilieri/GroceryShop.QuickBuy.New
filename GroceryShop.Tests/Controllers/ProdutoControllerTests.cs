using FluentAssertions;
using GroceryShop.Angular.Controllers;
using GroceryShop.Dominio.Contratos;
using GroceryShop.Dominio.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Xunit;

namespace GroceryShop.Tests.Controllers
{
    public class ProdutoControllerTests
    {
        private readonly Mock<IProdutoRepositorio> _mockRepositorio;
        private readonly Mock<IHttpContextAccessor> _mockHttpContext;
        private readonly Mock<IWebHostEnvironment> _mockEnvironment;
        private readonly ProdutoController _controller;

        public ProdutoControllerTests()
        {
            _mockRepositorio = new Mock<IProdutoRepositorio>();
            _mockHttpContext = new Mock<IHttpContextAccessor>();
            _mockEnvironment = new Mock<IWebHostEnvironment>();
            
            _controller = new ProdutoController(
                _mockRepositorio.Object,
                _mockHttpContext.Object,
                _mockEnvironment.Object
            );
        }

        [Fact]
        public void Get_DeveRetornarOkComListaDeProdutos()
        {
            // Arrange
            var produtos = new List<Produto>
            {
                new Produto { Id = 1, Nome = "Produto 1", Preco = 10.00m },
                new Produto { Id = 2, Nome = "Produto 2", Preco = 20.00m }
            };

            _mockRepositorio.Setup(r => r.ObterTodos()).Returns(produtos);

            // Act
            var result = _controller.Get();

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            
            var produtosRetornados = okResult.Value as IEnumerable<Produto>;
            produtosRetornados.Should().HaveCount(2);
        }

        [Fact]
        public void Get_QuandoOcorreErro_DeveRetornarBadRequest()
        {
            // Arrange
            _mockRepositorio.Setup(r => r.ObterTodos())
                .Throws(new Exception("Erro ao buscar produtos"));

            // Act
            var result = _controller.Get();

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(400);
        }

        [Fact]
        public void Post_ComProdutoValido_DeveRetornarCreated()
        {
            // Arrange
            var produto = new Produto
            {
                Nome = "Novo Produto",
                Descricao = "Descrição",
                Preco = 15.00m
            };

            _mockRepositorio.Setup(r => r.Adicionar(It.IsAny<Produto>()));

            // Act
            var result = _controller.Post(produto);

            // Assert
            result.Should().NotBeNull();
            _mockRepositorio.Verify(r => r.Adicionar(produto), Times.Once);
        }

        [Fact]
        public void Post_QuandoOcorreErro_DeveRetornarBadRequest()
        {
            // Arrange
            var produto = new Produto { Nome = "Produto", Preco = 10.00m };
            
            _mockRepositorio.Setup(r => r.Adicionar(It.IsAny<Produto>()))
                .Throws(new Exception("Erro ao adicionar"));

            // Act
            var result = _controller.Post(produto);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
        }

        [Fact]
        public void Delete_ComIdValido_DeveRemoverProduto()
        {
            // Arrange
            var produto = new Produto { Id = 1, Nome = "Produto", Preco = 10.00m };
            
            _mockRepositorio.Setup(r => r.ObterPorId(1)).Returns(produto);
            _mockRepositorio.Setup(r => r.Remover(It.IsAny<Produto>()));
            _mockRepositorio.Setup(r => r.ObterTodos()).Returns(new List<Produto>());

            // Act
            var result = _controller.Delete(1);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            _mockRepositorio.Verify(r => r.Remover(produto), Times.Once);
        }

        [Fact]
        public void Delete_ComIdInvalido_DeveRetornarNotFound()
        {
            // Arrange
            _mockRepositorio.Setup(r => r.ObterPorId(999)).Returns((Produto)null!);

            // Act
            var result = _controller.Delete(999);

            // Assert
            var notFoundResult = result as NotFoundResult;
            notFoundResult.Should().NotBeNull();
        }
    }
}
