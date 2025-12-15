using FluentAssertions;
using GroceryShop.Dominio.ObjetoDeValor;
using Xunit;

namespace GroceryShop.Tests.Dominio
{
    public class FormaPagamentoTests
    {
        [Fact]
        public void FormaPagamento_DeveInicializarPropriedadesCorretamente()
        {
            // Arrange & Act
            var formaPagamento = new FormaPagamento
            {
                Id = 1,
                Nome = "Cartão de Crédito",
                Descricao = "Pagamento via cartão de crédito"
            };

            // Assert
            formaPagamento.Id.Should().Be(1);
            formaPagamento.Nome.Should().Be("Cartão de Crédito");
            formaPagamento.Descricao.Should().Be("Pagamento via cartão de crédito");
        }

        [Fact]
        public void FormaPagamento_DeveCriarBoleto()
        {
            // Arrange & Act
            var boleto = new FormaPagamento
            {
                Id = 1,
                Nome = "Boleto",
                Descricao = "Forma de pagamento Boleto"
            };

            // Assert
            boleto.Nome.Should().Be("Boleto");
        }

        [Fact]
        public void FormaPagamento_DeveCriarCartaoCredito()
        {
            // Arrange & Act
            var cartao = new FormaPagamento
            {
                Id = 2,
                Nome = "Cartão de Crédito",
                Descricao = "Forma de pagamento Cartão de Crédito"
            };

            // Assert
            cartao.Nome.Should().Be("Cartão de Crédito");
        }

        [Fact]
        public void FormaPagamento_DeveCriarDeposito()
        {
            // Arrange & Act
            var deposito = new FormaPagamento
            {
                Id = 3,
                Nome = "Depósito",
                Descricao = "Forma de pagamento Depósito"
            };

            // Assert
            deposito.Nome.Should().Be("Depósito");
        }

        [Fact]
        public void FormaPagamento_DevePermitirDescricaoVazia()
        {
            // Arrange & Act
            var formaPagamento = new FormaPagamento
            {
                Nome = "PIX",
                Descricao = string.Empty
            };

            // Assert
            formaPagamento.Descricao.Should().BeEmpty();
        }
    }
}
