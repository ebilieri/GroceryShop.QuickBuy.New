using FluentAssertions;
using GroceryShop.Dominio.Entidades;
using GroceryShop.Repositorio.Contexto;
using GroceryShop.Repositorio.Repositorios;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GroceryShop.Tests.Repositorio
{
    public class ProdutoRepositorioTests
    {
        private QuickBuyContexto CriarContextoEmMemoria()
        {
            var options = new DbContextOptionsBuilder<QuickBuyContexto>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new QuickBuyContexto(options);
        }

        [Fact]
        public void Adicionar_DeveSalvarProdutoNoBanco()
        {
            // Arrange
            using var context = CriarContextoEmMemoria();
            var repositorio = new ProdutoRepositorio(context);
            var produto = new Produto
            {
                Nome = "Produto Teste",
                Descricao = "Descrição do produto teste",
                Preco = 10.50m,
                NomeArquivo = "produto.jpg"
            };

            // Act
            repositorio.Adicionar(produto);

            // Assert
            var produtoSalvo = context.Produtos.FirstOrDefault(p => p.Nome == "Produto Teste");
            produtoSalvo.Should().NotBeNull();
            produtoSalvo!.Nome.Should().Be("Produto Teste");
            produtoSalvo.Preco.Should().Be(10.50m);
        }

        [Fact]
        public void ObterTodos_DeveRetornarTodosOsProdutos()
        {
            // Arrange
            using var context = CriarContextoEmMemoria();
            var repositorio = new ProdutoRepositorio(context);

            var produtos = new List<Produto>
            {
                new Produto { Nome = "Produto 1", Descricao = "Desc 1", Preco = 10.00m },
                new Produto { Nome = "Produto 2", Descricao = "Desc 2", Preco = 20.00m },
                new Produto { Nome = "Produto 3", Descricao = "Desc 3", Preco = 30.00m }
            };

            foreach (var produto in produtos)
            {
                context.Produtos.Add(produto);
            }
            context.SaveChanges();

            // Act
            var resultado = repositorio.ObterTodos();

            // Assert
            resultado.Should().HaveCount(3);
        }

        [Fact]
        public void ObterPorId_DeveRetornarProdutoCorreto()
        {
            // Arrange
            using var context = CriarContextoEmMemoria();
            var repositorio = new ProdutoRepositorio(context);
            var produto = new Produto
            {
                Nome = "Produto Específico",
                Descricao = "Produto para teste",
                Preco = 15.00m
            };
            context.Produtos.Add(produto);
            context.SaveChanges();

            // Act
            var resultado = repositorio.ObterPorId(produto.Id);

            // Assert
            resultado.Should().NotBeNull();
            resultado!.Nome.Should().Be("Produto Específico");
        }

        [Fact]
        public void Atualizar_DeveModificarProdutoExistente()
        {
            // Arrange
            using var context = CriarContextoEmMemoria();
            var repositorio = new ProdutoRepositorio(context);
            var produto = new Produto
            {
                Nome = "Produto Original",                Descricao = "Descrição original",                Preco = 10.00m
            };
            context.Produtos.Add(produto);
            context.SaveChanges();

            // Act
            produto.Nome = "Produto Atualizado";
            produto.Preco = 20.00m;
            repositorio.Atualizar(produto);

            // Assert
            var produtoAtualizado = context.Produtos.Find(produto.Id);
            produtoAtualizado!.Nome.Should().Be("Produto Atualizado");
            produtoAtualizado.Preco.Should().Be(20.00m);
        }

        [Fact]
        public void Remover_DeveExcluirProdutoDoBanco()
        {
            // Arrange
            using var context = CriarContextoEmMemoria();
            var repositorio = new ProdutoRepositorio(context);
            var produto = new Produto
            {
                Nome = "Produto a Remover",                Descricao = "Descrição do produto",                Preco = 10.00m
            };
            context.Produtos.Add(produto);
            context.SaveChanges();
            var produtoId = produto.Id;

            // Act
            repositorio.Remover(produto);

            // Assert
            var produtoRemovido = context.Produtos.Find(produtoId);
            produtoRemovido.Should().BeNull();
        }
    }
}
