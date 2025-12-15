using FluentAssertions;
using GroceryShop.Dominio.Entidades;
using GroceryShop.Repositorio.Contexto;
using GroceryShop.Repositorio.Repositorios;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GroceryShop.Tests.Repositorio
{
    public class UsuarioRepositorioTests
    {
        private QuickBuyContexto CriarContextoEmMemoria()
        {
            var options = new DbContextOptionsBuilder<QuickBuyContexto>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new QuickBuyContexto(options);
        }

        [Fact]
        public void Adicionar_DeveSalvarUsuarioNoBanco()
        {
            // Arrange
            using var context = CriarContextoEmMemoria();
            var repositorio = new UsuarioRepositorio(context);
            var usuario = new Usuario
            {
                Email = "teste@teste.com",
                Senha = "senha123",
                Nome = "João",
                SobreNome = "Silva"
            };

            // Act
            repositorio.Adicionar(usuario);

            // Assert
            var usuarioSalvo = context.Usuarios.FirstOrDefault(u => u.Email == "teste@teste.com");
            usuarioSalvo.Should().NotBeNull();
            usuarioSalvo!.Nome.Should().Be("João");
            usuarioSalvo.SobreNome.Should().Be("Silva");
        }

        [Fact]
        public void Obter_ComEmailESenhaValidos_DeveRetornarUsuario()
        {
            // Arrange
            using var context = CriarContextoEmMemoria();
            var repositorio = new UsuarioRepositorio(context);
            var usuario = new Usuario
            {
                Email = "teste@teste.com",
                Senha = "senha123",
                Nome = "Maria",
                SobreNome = "Santos"
            };
            context.Usuarios.Add(usuario);
            context.SaveChanges();

            // Act
            var resultado = repositorio.Obter("teste@teste.com", "senha123");

            // Assert
            resultado.Should().NotBeNull();
            resultado!.Email.Should().Be("teste@teste.com");
            resultado.Nome.Should().Be("Maria");
        }

        [Fact]
        public void Obter_ComCredenciaisInvalidas_DeveRetornarNull()
        {
            // Arrange
            using var context = CriarContextoEmMemoria();
            var repositorio = new UsuarioRepositorio(context);
            var usuario = new Usuario
            {
                Email = "teste@teste.com",
                Senha = "senha123",
                Nome = "Pedro",
                SobreNome = "Oliveira"
            };
            context.Usuarios.Add(usuario);
            context.SaveChanges();

            // Act
            var resultado = repositorio.Obter("teste@teste.com", "senhaErrada");

            // Assert
            resultado.Should().BeNull();
        }

        [Fact]
        public void ObterTodos_DeveRetornarTodosOsUsuarios()
        {
            // Arrange
            using var context = CriarContextoEmMemoria();
            var repositorio = new UsuarioRepositorio(context);

            var usuarios = new List<Usuario>
            {
                new Usuario { Email = "user1@test.com", Senha = "123", Nome = "User", SobreNome = "One" },
                new Usuario { Email = "user2@test.com", Senha = "456", Nome = "User", SobreNome = "Two" },
                new Usuario { Email = "user3@test.com", Senha = "789", Nome = "User", SobreNome = "Three" }
            };

            foreach (var usuario in usuarios)
            {
                context.Usuarios.Add(usuario);
            }
            context.SaveChanges();

            // Act
            var resultado = repositorio.ObterTodos();

            // Assert
            resultado.Should().HaveCount(3);
        }

        [Fact]
        public void Atualizar_DeveModificarUsuarioExistente()
        {
            // Arrange
            using var context = CriarContextoEmMemoria();
            var repositorio = new UsuarioRepositorio(context);
            var usuario = new Usuario
            {
                Email = "teste@teste.com",
                Senha = "senha123",
                Nome = "Nome Original",
                SobreNome = "Sobrenome Original"
            };
            context.Usuarios.Add(usuario);
            context.SaveChanges();

            // Act
            usuario.Nome = "Nome Atualizado";
            usuario.SobreNome = "Sobrenome Atualizado";
            repositorio.Atualizar(usuario);

            // Assert
            var usuarioAtualizado = context.Usuarios.Find(usuario.Id);
            usuarioAtualizado!.Nome.Should().Be("Nome Atualizado");
            usuarioAtualizado.SobreNome.Should().Be("Sobrenome Atualizado");
        }

        [Fact]
        public void Remover_DeveExcluirUsuarioDoBanco()
        {
            // Arrange
            using var context = CriarContextoEmMemoria();
            var repositorio = new UsuarioRepositorio(context);
            var usuario = new Usuario
            {
                Email = "remover@teste.com",
                Senha = "senha123",
                Nome = "Usuario",
                SobreNome = "Remover"
            };
            context.Usuarios.Add(usuario);
            context.SaveChanges();
            var usuarioId = usuario.Id;

            // Act
            repositorio.Remover(usuario);

            // Assert
            var usuarioRemovido = context.Usuarios.Find(usuarioId);
            usuarioRemovido.Should().BeNull();
        }
    }
}
