import { Produto } from "../../modelo/produto";

export class LojaCarrinhoCompras {
    

  public produtos: Produto[] = [];

  public adicionar(produto: Produto) {
    var produtosLocalStorage = localStorage.getItem("produtosLocalStorage");

    // Garantir que o produto tenha quantidade definida
    if (!produto.quantidade || produto.quantidade === 0) {
      produto.quantidade = 1;
    }

    // Preservar o preço original (unitário) do produto
    if (!produto.precoOriginal) {
      produto.precoOriginal = produto.preco;
    }

    if (!produtosLocalStorage) {
      // se não existir nada dentro do localStorage
      this.produtos.push(produto);
    } else {
      // se ja existir pelo menos um item armazenado na sessao(produtosLocalStorage)      
      this.produtos = JSON.parse(produtosLocalStorage);

      // Verificar se o produto já existe no carrinho
      var produtoExistente = this.produtos.find(p => p.id == produto.id);
      
      if (produtoExistente) {
        // Se o produto já existe, incrementar a quantidade
        produtoExistente.quantidade += produto.quantidade;
      } else {
        // Se não existe, adicionar como novo item
        this.produtos.push(produto);
      }
    }

    localStorage.setItem("produtosLocalStorage", JSON.stringify(this.produtos));
  }

  public obterProdutos(): Produto[] {
    var produtosLocalStorage = localStorage.getItem("produtosLocalStorage");

    if (produtosLocalStorage)
      return JSON.parse(produtosLocalStorage);

    return this.produtos;
  }

  public removerProduto(produto: Produto) {
    var produtosLocalStorage = localStorage.getItem("produtosLocalStorage");

    if (produtosLocalStorage) {
      // recuperar os dodos do local storage
      this.produtos = JSON.parse(produtosLocalStorage);
      //retornar todos os produtos diferentes do atual
      this.produtos = this.produtos.filter(p => p.id != produto.id);
      // atualizar local storage
      localStorage.setItem("produtosLocalStorage", JSON.stringify(this.produtos));
    }
  }

  public atualizar(produtos: Produto[]) {
    localStorage.setItem("produtosLocalStorage", JSON.stringify(produtos));
  }

  public temItensCarrinhoCompras(): boolean {
    var itens = this.obterProdutos();
    return (itens.length > 0);
  }

 public limparCarrinhoComprar() {
   localStorage.setItem("produtosLocalStorage", "");
  }
}
