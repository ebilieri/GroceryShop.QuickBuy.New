using FluentAssertions;
using GroceryShop.Dominio.Entidades;
using Xunit;

namespace GroceryShop.Tests.Dominio
{
    public class ProdutoTests
    {
        [Fact]
        public void Produto_DeveInicializarPropriedadesCorretamente()
        {
            // Arrange & Act
            var produto = new Produto
            {
                Id = 1,
                Nome = "Arroz",
                Descricao = "Arroz branco tipo 1",
                Preco = 25.90m,
                NomeArquivo = "arroz.jpg"
            };

            // Assert
            produto.Id.Should().Be(1);
            produto.Nome.Should().Be("Arroz");
            produto.Descricao.Should().Be("Arroz branco tipo 1");
            produto.Preco.Should().Be(25.90m);
            produto.NomeArquivo.Should().Be("arroz.jpg");
        }

        [Fact]
        public void Produto_DevePermitirPrecoZero()
        {
            // Arrange & Act
            var produto = new Produto
            {
                Nome = "Produto Gratuito",
                Preco = 0
            };

            // Assert
            produto.Preco.Should().Be(0);
        }

        [Fact]
        public void Produto_DevePermitirDescricaoVazia()
        {
            // Arrange & Act
            var produto = new Produto
            {
                Nome = "Produto",
                Descricao = string.Empty
            };

            // Assert
            produto.Descricao.Should().BeEmpty();
        }

        [Fact]
        public void Produto_DeveTerIdHerdadoDaEntidadeBase()
        {
            // Arrange & Act
            var produto = new Produto { Id = 123 };

            // Assert
            produto.Should().BeAssignableTo<Entidade>();
            produto.Id.Should().Be(123);
        }
    }
}
