using FluentAssertions;
using GroceryShop.Dominio.Entidades;
using GroceryShop.Dominio.ObjetoDeValor;
using GroceryShop.Repositorio.Contexto;
using GroceryShop.Repositorio.Repositorios;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GroceryShop.Tests.Repositorio
{
    public class PedidoRepositorioTests
    {
        private QuickBuyContexto CriarContextoEmMemoria()
        {
            var options = new DbContextOptionsBuilder<QuickBuyContexto>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new QuickBuyContexto(options);
            
            // Seed data for FormaPagamento if needed
            if (!context.FormaPagamentos.Any())
            {
                context.FormaPagamentos.AddRange(
                    new FormaPagamento { Id = 1, Nome = "Boleto", Descricao = "Boleto Bancário" },
                    new FormaPagamento { Id = 2, Nome = "Cartão", Descricao = "Cartão de Crédito" }
                );
                context.SaveChanges();
            }

            return context;
        }

        [Fact]
        public void Adicionar_DeveSalvarPedidoNoBanco()
        {
            // Arrange
            using var context = CriarContextoEmMemoria();
            var repositorio = new PedidoRepositorio(context);

            var usuario = new Usuario
            {
                Email = "teste@teste.com",
                Senha = "senha",
                Nome = "João",
                SobreNome = "Silva"
            };
            context.Usuarios.Add(usuario);
            context.SaveChanges();

            var pedido = new Pedido
            {
                DataPedido = DateTime.Now,
                UsuarioId = usuario.Id,
                FormaPagamentoId = 1,                CEP = "12345-678",
                Estado = "SP",
                Cidade = "São Paulo",
                EnderecoCompleto = "Rua Teste, 123",                ItensPedidos = new List<ItemPedido>()
            };

            // Act
            repositorio.Adicionar(pedido);

            // Assert
            var pedidoSalvo = context.Pedidos.FirstOrDefault();
            pedidoSalvo.Should().NotBeNull();
            pedidoSalvo!.UsuarioId.Should().Be(usuario.Id);
        }

        [Fact]
        public void ObterTodos_DeveRetornarTodosOsPedidos()
        {
            // Arrange
            using var context = CriarContextoEmMemoria();
            var repositorio = new PedidoRepositorio(context);

            var usuario = new Usuario
            {
                Email = "teste@teste.com",
                Senha = "senha",
                Nome = "Maria",
                SobreNome = "Santos"
            };
            context.Usuarios.Add(usuario);
            context.SaveChanges();

            var pedidos = new List<Pedido>
            {
                new Pedido { DataPedido = DateTime.Now, UsuarioId = usuario.Id, FormaPagamentoId = 1, CEP = "11111-111", Estado = "SP", Cidade = "São Paulo", EnderecoCompleto = "Rua A, 1" },
                new Pedido { DataPedido = DateTime.Now, UsuarioId = usuario.Id, FormaPagamentoId = 2, CEP = "22222-222", Estado = "RJ", Cidade = "Rio de Janeiro", EnderecoCompleto = "Rua B, 2" }
            };

            foreach (var pedido in pedidos)
            {
                context.Pedidos.Add(pedido);
            }
            context.SaveChanges();

            // Act
            var resultado = repositorio.ObterTodos();

            // Assert
            resultado.Should().HaveCount(2);
        }

        [Fact]
        public void ObterPorId_DeveRetornarPedidoCorreto()
        {
            // Arrange
            using var context = CriarContextoEmMemoria();
            var repositorio = new PedidoRepositorio(context);

            var usuario = new Usuario
            {
                Email = "teste@teste.com",
                Senha = "senha",
                Nome = "Pedro",
                SobreNome = "Oliveira"
            };
            context.Usuarios.Add(usuario);
            context.SaveChanges();

            var pedido = new Pedido
            {
                DataPedido = DateTime.Now,
                UsuarioId = usuario.Id,
                FormaPagamentoId = 1,
                CEP = "33333-333",
                Estado = "MG",
                Cidade = "Belo Horizonte",
                EnderecoCompleto = "Rua C, 3"
            };
            context.Pedidos.Add(pedido);
            context.SaveChanges();

            // Act
            var resultado = repositorio.ObterPorId(pedido.Id);

            // Assert
            resultado.Should().NotBeNull();
            resultado!.Id.Should().Be(pedido.Id);
        }

        [Fact]
        public void Atualizar_DeveModificarPedidoExistente()
        {
            // Arrange
            using var context = CriarContextoEmMemoria();
            var repositorio = new PedidoRepositorio(context);

            var usuario = new Usuario
            {
                Email = "teste@teste.com",
                Senha = "senha",
                Nome = "Ana",
                SobreNome = "Costa"
            };
            context.Usuarios.Add(usuario);
            context.SaveChanges();

            var pedido = new Pedido
            {
                DataPedido = DateTime.Now,
                UsuarioId = usuario.Id,
                FormaPagamentoId = 1,
                CEP = "44444-444",
                Estado = "RS",
                Cidade = "Porto Alegre",
                EnderecoCompleto = "Rua D, 4"
            };
            context.Pedidos.Add(pedido);
            context.SaveChanges();

            // Act
            pedido.FormaPagamentoId = 2;
            repositorio.Atualizar(pedido);

            // Assert
            var pedidoAtualizado = context.Pedidos.Find(pedido.Id);
            pedidoAtualizado!.FormaPagamentoId.Should().Be(2);
        }

        [Fact]
        public void Remover_DeveExcluirPedidoDoBanco()
        {
            // Arrange
            using var context = CriarContextoEmMemoria();
            var repositorio = new PedidoRepositorio(context);

            var usuario = new Usuario
            {
                Email = "teste@teste.com",
                Senha = "senha",
                Nome = "Carlos",
                SobreNome = "Lima"
            };
            context.Usuarios.Add(usuario);
            context.SaveChanges();

            var pedido = new Pedido
            {
                DataPedido = DateTime.Now,
                UsuarioId = usuario.Id,
                FormaPagamentoId = 1,
                CEP = "55555-555",
                Estado = "PR",
                Cidade = "Curitiba",
                EnderecoCompleto = "Rua E, 5"
            };
            context.Pedidos.Add(pedido);
            context.SaveChanges();
            var pedidoId = pedido.Id;

            // Act
            repositorio.Remover(pedido);

            // Assert
            var pedidoRemovido = context.Pedidos.Find(pedidoId);
            pedidoRemovido.Should().BeNull();
        }
    }
}
