using FluentAssertions;
using GroceryShop.Angular.Controllers;
using GroceryShop.Dominio.Contratos;
using GroceryShop.Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GroceryShop.Tests.Controllers
{
    public class UsuarioControllerTests
    {
        private readonly Mock<IUsuarioRepositorio> _mockRepositorio;
        private readonly UsuarioController _controller;

        public UsuarioControllerTests()
        {
            _mockRepositorio = new Mock<IUsuarioRepositorio>();
            _controller = new UsuarioController(_mockRepositorio.Object);
        }

        [Fact]
        public void Get_DeveRetornarOk()
        {
            // Act
            var result = _controller.Get();

            // Assert
            var okResult = result as OkResult;
            okResult.Should().NotBeNull();
        }

        [Fact]
        public void Post_ComUsuarioNovo_DeveRetornarCreated()
        {
            // Arrange
            var usuario = new Usuario
            {
                Email = "novo@test.com",
                Senha = "senha123",
                Nome = "Novo",
                SobreNome = "Usuario"
            };

            _mockRepositorio.Setup(r => r.Obter(usuario.Email)).Returns((Usuario)null!);
            _mockRepositorio.Setup(r => r.Adicionar(It.IsAny<Usuario>()));

            // Act
            var result = _controller.Post(usuario);

            // Assert
            var createdResult = result as CreatedResult;
            createdResult.Should().NotBeNull();
            _mockRepositorio.Verify(r => r.Adicionar(usuario), Times.Once);
        }

        [Fact]
        public void Post_ComUsuarioExistente_DeveRetornarBadRequest()
        {
            // Arrange
            var usuarioExistente = new Usuario
            {
                Id = 1,
                Email = "existente@test.com",
                Senha = "senha",
                Nome = "Usuario",
                SobreNome = "Existente"
            };

            _mockRepositorio.Setup(r => r.Obter(usuarioExistente.Email)).Returns(usuarioExistente);

            // Act
            var result = _controller.Post(usuarioExistente);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult!.Value.Should().Be("Usuário já cadastrado no sistema com esse Email");
        }

        [Fact]
        public void Post_QuandoOcorreErro_DeveRetornarBadRequest()
        {
            // Arrange
            var usuario = new Usuario { Email = "teste@test.com", Senha = "senha" };
            
            _mockRepositorio.Setup(r => r.Obter(It.IsAny<string>())).Returns((Usuario)null!);
            _mockRepositorio.Setup(r => r.Adicionar(It.IsAny<Usuario>()))
                .Throws(new Exception("Erro ao adicionar"));

            // Act
            var result = _controller.Post(usuario);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
        }

        [Fact]
        public void VerificarUsuario_ComCredenciaisValidas_DeveRetornarOk()
        {
            // Arrange
            var usuario = new Usuario
            {
                Id = 1,
                Email = "user@test.com",
                Senha = "senha123",
                Nome = "Usuario",
                SobreNome = "Teste"
            };

            _mockRepositorio.Setup(r => r.Obter(usuario.Email, usuario.Senha)).Returns(usuario);

            // Act
            var result = _controller.VerificarUsuario(usuario);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var usuarioRetornado = okResult!.Value as Usuario;
            usuarioRetornado.Should().NotBeNull();
            usuarioRetornado!.Email.Should().Be(usuario.Email);
        }

        [Fact]
        public void VerificarUsuario_ComCredenciaisInvalidas_DeveRetornarBadRequest()
        {
            // Arrange
            var usuario = new Usuario
            {
                Email = "wrong@test.com",
                Senha = "wrongpassword"
            };

            _mockRepositorio.Setup(r => r.Obter(usuario.Email, usuario.Senha)).Returns((Usuario)null!);

            // Act
            var result = _controller.VerificarUsuario(usuario);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult!.Value.Should().Be("Usuário ou senha inválido");
        }
    }
}
