# GroceryShop.QuickBuy

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=.net)
![Angular](https://img.shields.io/badge/Angular-19.0.6-DD0031?logo=angular)
![TypeScript](https://img.shields.io/badge/TypeScript-5.6.3-3178C6?logo=typescript)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3.3-7952B3?logo=bootstrap)
![Tests](https://img.shields.io/badge/Tests-54%20passed-success?logo=xunit)
![Coverage](https://img.shields.io/badge/Coverage-80--85%25-green)

Aplicação de e-commerce fullstack desenvolvida com ASP.NET Core e Angular, implementando uma loja virtual completa com gerenciamento de produtos, usuários, carrinho de compras e pedidos.

## 📋 Índice

- [Sobre o Projeto](#sobre-o-projeto)
- [Tecnologias](#tecnologias)
- [Testes](#testes)
- [Arquitetura](#arquitetura)
- [Pré-requisitos](#pré-requisitos)
- [Instalação](#instalação)
- [Executando a Aplicação](#executando-a-aplicação)
- [Documentação Técnica](#documentação-técnica)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Testes Unitários](#testes-unitários)
- [API Endpoints](#api-endpoints)
- [Contribuição](#contribuição)

## 🎯 Sobre o Projeto

O GroceryShop.QuickBuy é uma aplicação fullstack que simula um e-commerce completo, permitindo:
- Cadastro e autenticação de usuários
- Listagem e pesquisa de produtos
- Gerenciamento de carrinho de compras com persistência local
- Finalização de pedidos
- Upload de imagens de produtos
- Painel administrativo para gestão de produtos

## 🚀 Tecnologias

### Backend
- **.NET 10.0** - Framework principal
- **ASP.NET Core Web API** - APIs RESTful
- **Entity Framework Core 9.0.1** - ORM para acesso a dados
- **MySQL** - Banco de dados relacional
- **Swagger/OpenAPI** - Documentação de API
- **Newtonsoft.Json** - Serialização JSON

### Frontend
- **Angular 19.0.6** - Framework SPA
- **TypeScript 5.6.3** - Linguagem de programação
- **Bootstrap 5.3.3** - Framework CSS
- **ngx-toastr 19.0.0** - Notificações toast
- **RxJS 7.8.1** - Programação reativa
- **Angular CLI 19.0.6** - Ferramentas de desenvolvimento

## 🧪 Testes

O projeto possui uma suíte completa de **54 testes unitários** cobrindo Controllers, Repositórios e Entidades de Domínio.

### Executar Testes
```bash
cd GroceryShop.Tests
dotnet test
```

### Cobertura de Código
```bash
# Executar testes com cobertura
dotnet test --collect:"XPlat Code Coverage"

# Gerar relatório HTML
reportgenerator -reports:"TestResults\*\coverage.cobertura.xml" -targetdir:"CoverageReport" -reporttypes:Html
```

### Estatísticas
- ✅ **54 testes** - 100% de aprovação
- 📊 **Cobertura Global**: ~23% (incluindo infraestrutura)
- 🎯 **Cobertura de Código Testável**: ~80-85%
- ⚡ **Tempo de Execução**: ~5.4 segundos

Ver relatório completo: [TESTE_REPORT.md](TESTE_REPORT.md)

---

## 🏗️ Arquitetura

A aplicação segue uma arquitetura em camadas (Clean Architecture) com separação de responsabilidades:

```
GroceryShop.QuickBuy/
│
├── GroceryShop.Angular/          # Camada de Apresentação
│   ├── Controllers/              # API Controllers
│   ├── ClientApp/                # Aplicação Angular
│   │   ├── src/app/
│   │   │   ├── loja/            # Módulo da Loja
│   │   │   ├── produto/         # Módulo de Produtos
│   │   │   ├── usuario/         # Módulo de Usuários
│   │   │   ├── modelo/          # Modelos TypeScript
│   │   │   └── servicos/        # Serviços HTTP
│   └── wwwroot/                  # Arquivos estáticos
│
├── GroceryShop.Dominio/          # Camada de Domínio
│   ├── Entidades/                # Entidades de negócio
│   ├── Contratos/                # Interfaces de repositório
│   ├── ObjetoDeValor/            # Value Objects
│   └── Enumeradores/             # Enumerações
│
└── GroceryShop.Repositorio/      # Camada de Dados
    ├── Contexto/                 # DbContext
    ├── Repositorios/             # Implementações de repositórios
    ├── Config/                   # Configurações EF Core
    └── Migrations/               # Migrações do banco
```

### Padrões Utilizados
- **Repository Pattern** - Abstração da camada de dados
- **Dependency Injection** - Inversão de controle
- **DTO Pattern** - Transferência de dados entre camadas
- **Component-Based Architecture** - Frontend modular
- **NgModule Architecture** - Organização Angular tradicional

## 📦 Pré-requisitos

- [.NET SDK 10.0](https://dotnet.microsoft.com/download)
- [Node.js 20.x ou superior](https://nodejs.org/)
- [MySQL 8.0 ou superior](https://www.mysql.com/downloads/)
- [Angular CLI 19.x](https://angular.io/cli)
- Visual Studio 2022 ou VS Code (recomendado)

## 🔧 Instalação

### 1. Clone o repositório
```bash
git clone https://github.com/ebilieri/GroceryShop.QuickBuy.New.git
cd GroceryShop.QuickBuy.New
```

### 2. Configure o Banco de Dados

Edite o arquivo `GroceryShop.Angular/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "QuickByConnection": "server=localhost;userid=root;password=SUA_SENHA;database=QuickBuyAngularDB"
  }
}
```

### 3. Execute as Migrações
```bash
cd GroceryShop.Angular
dotnet ef database update
```

### 4. Instale as Dependências do Frontend
```bash
cd ClientApp
npm install --legacy-peer-deps
```

## ▶️ Executando a Aplicação

### Opção 1: Aplicação Completa (Backend + Frontend)
```bash
cd GroceryShop.Angular
dotnet run
```
A aplicação estará disponível em:
- Backend: https://localhost:5001
- Frontend (integrado): https://localhost:5001

### Opção 2: Desenvolvimento Separado

**Backend:**
```bash
cd GroceryShop.Angular
dotnet run
```

**Frontend:**
```bash
cd GroceryShop.Angular/ClientApp
npm start
```
- Backend: https://localhost:5001
- Frontend: http://localhost:4200

### Build de Produção

**Backend:**
```bash
cd GroceryShop.Angular
dotnet publish -c Release
```

**Frontend:**
```bash
cd GroceryShop.Angular/ClientApp
npm run build
```

## 📚 Documentação Técnica

### Backend (.NET Core)

#### Controllers

**ProdutoController** (`/api/produto`)
- `GET /api/produto` - Lista todos os produtos
- `POST /api/produto` - Cadastra novo produto
- `DELETE /api/produto/{id}` - Remove produto
- `POST /api/produto/EnviarArquivo` - Upload de imagem

**UsuarioController** (`/api/usuario`)
- `GET /api/usuario` - Lista usuários
- `POST /api/usuario` - Cadastra usuário
- `POST /api/usuario/VerificarUsuario` - Autentica usuário

**PedidoController** (`/api/pedido`)
- `POST /api/pedido` - Finaliza pedido

#### Entidades Principais

**Produto**
```csharp
{
  Id: int,
  Nome: string,
  Descricao: string,
  Preco: decimal,
  NomeArquivo: string
}
```

**Usuario**
```csharp
{
  Id: int,
  Email: string,
  Senha: string,
  Nome: string,
  SobreNome: string
}
```

**Pedido**
```csharp
{
  Id: int,
  DataPedido: DateTime,
  UsuarioId: int,
  FormaPagamentoId: int,
  ItensPedido: List<ItemPedido>
}
```

### Frontend (Angular)

#### Módulos Principais

**LojaModule**
- `loja.pesquisa.component` - Página de pesquisa de produtos
- `loja.produto.component` - Detalhes do produto
- `loja.efetivar.component` - Carrinho de compras
- `loja.compra.finalizada.component` - Confirmação de pedido

**ProdutoModule**
- `produto.component` - Gerenciamento de produtos (admin)
- `pesquisa.produto.component` - Busca administrativa

**UsuarioModule**
- `login.component` - Autenticação
- `cadastro.usuario.component` - Registro de usuário

#### Serviços

**ProdutoServico**
```typescript
obterTodosProdutos(): Observable<Produto[]>
cadastrar(produto: Produto): Observable<Produto>
deletar(produto: Produto): Observable<Produto>
enviarArquivo(arquivoSelecionado: File): Observable<string>
```

**UsuarioServico**
```typescript
verificarUsuario(usuario: Usuario): Observable<Usuario>
cadastrar(usuario: Usuario): Observable<Usuario>
get usuario(): Usuario
limparSessao(): void
```

**PedidoServico**
```typescript
cadastrar(pedido: Pedido): Observable<number>
```

#### Gestão de Estado

**LojaCarrinhoCompras** (LocalStorage)
```typescript
class LojaCarrinhoCompras {
  adicionar(produto: Produto): void
  removerProduto(produto: Produto): void
  obterProdutos(): Produto[]
  temItensCarrinho(): boolean
  limpar(): void
}
```

## 🗂️ Estrutura do Projeto

### Backend
```
GroceryShop.Angular/
├── Controllers/           # Endpoints da API
├── Pages/                # Páginas Razor (fallback)
├── Properties/           # Configurações de lançamento
├── ClientApp/            # Aplicação Angular
└── wwwroot/             # Arquivos estáticos
    └── arquivos/        # Upload de imagens

GroceryShop.Dominio/
├── Contratos/           # Interfaces de repositório
├── Entidades/           # Modelos de domínio
├── Enumeradores/        # Enums
└── ObjetoDeValor/       # Value objects

GroceryShop.Repositorio/
├── Contexto/            # DbContext do EF Core
├── Config/              # Fluent API configurations
├── Migrations/          # Migrações do banco
└── Repositorios/        # Implementações de repositório
```

### Frontend
```
ClientApp/src/app/
├── loja/
│   ├── efetivar/              # Carrinho e checkout
│   ├── pesquisa/              # Listagem de produtos
│   └── produto/               # Detalhes do produto
├── produto/
│   └── pesquisa/              # Admin - gestão de produtos
├── usuario/
│   ├── cadastro/              # Registro
│   └── login/                 # Autenticação
├── modelo/                    # Modelos TypeScript
├── servicos/                  # Serviços HTTP
└── pipes/                     # Pipes customizados
```

## 🧪 Testes Unitários

O projeto possui uma suíte completa de testes unitários com **54 testes** organizados em 3 categorias principais, garantindo a qualidade e confiabilidade do código.

### Estrutura de Testes

```
GroceryShop.Tests/
├── Controllers/                     # 15 testes
│   ├── ProdutoControllerTests.cs    # 6 testes
│   ├── UsuarioControllerTests.cs    # 6 testes
│   └── PedidoControllerTests.cs     # 3 testes
├── Repositorio/                     # 16 testes
│   ├── ProdutoRepositorioTests.cs   # 5 testes
│   ├── UsuarioRepositorioTests.cs   # 6 testes
│   └── PedidoRepositorioTests.cs    # 5 testes
└── Dominio/                         # 23 testes
    ├── ProdutoTests.cs              # 4 testes
    ├── UsuarioTests.cs              # 4 testes
    ├── PedidoTests.cs               # 5 testes
    ├── ItemPedidoTests.cs           # 5 testes
    └── FormaPagamentoTests.cs       # 5 testes
```

### Estatísticas de Testes

| Categoria        | Total de Testes | Status |
|-----------------|-----------------|--------|
| Controllers     | 15              | ✅ 100% |
| Repositórios    | 16              | ✅ 100% |
| Domínio         | 23              | ✅ 100% |
| **TOTAL**       | **54**          | ✅ **100%** |

### Cobertura de Código

- **Cobertura Global**: 22.96%
- **Cobertura de Código Testável**: ~80-85%
- **54 testes passando** (100% de sucesso)
- **0 testes falhando**

> 📊 **Nota**: A cobertura global inclui código gerado automaticamente (Migrations, Program.cs, Startup.cs). O código testável (Controllers, Repositórios, Domínio) possui cobertura acima de 80%.

### Tecnologias de Teste

- **xUnit** 3.1.4 - Framework de testes
- **Moq** 4.20.72 - Biblioteca de mocking
- **FluentAssertions** 8.8.0 - Asserções fluentes
- **EF Core InMemory** 9.0.1 - Banco de dados em memória
- **coverlet.collector** 6.0.4 - Coleta de cobertura
- **Microsoft.AspNetCore.Mvc.Testing** 10.0.1 - Testes de API

### Executando os Testes

**Rodar todos os testes:**
```bash
dotnet test
```

**Rodar com cobertura de código:**
```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
```

**Gerar relatório HTML de cobertura:**
```bash
# Instalar ReportGenerator (apenas uma vez)
dotnet tool install -g dotnet-reportgenerator-globaltool

# Coletar cobertura e gerar relatório
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
reportgenerator -reports:"GroceryShop.Tests/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html

# Abrir relatório
start coveragereport/index.html
```

**Rodar testes de uma categoria específica:**
```bash
# Apenas testes de Controllers
dotnet test --filter "FullyQualifiedName~Controllers"

# Apenas testes de Repositórios
dotnet test --filter "FullyQualifiedName~Repositorio"

# Apenas testes de Domínio
dotnet test --filter "FullyQualifiedName~Dominio"
```

### Exemplos de Testes

**Controllers:**
- Teste de listagem de produtos
- Teste de criação de produto com dados válidos
- Teste de validação de modelo inválido
- Teste de autenticação de usuário
- Teste de finalização de pedido

**Repositórios:**
- Teste de operações CRUD
- Teste de buscas e filtros
- Teste de persistência de dados
- Teste de relacionamentos entre entidades

**Domínio:**
- Teste de validação de entidades
- Teste de objetos de valor (FormaPagamento)
- Teste de regras de negócio
- Teste de cálculos (subtotal, total)

### Relatório Detalhado

Para informações completas sobre a suíte de testes, incluindo descrição de cada teste, estratégias de teste e análise de cobertura, consulte o [**TESTE_REPORT.md**](TESTE_REPORT.md).

## 🔌 API Endpoints

### Produtos
```
GET    /api/produto              - Lista produtos
POST   /api/produto              - Cria produto
DELETE /api/produto/{id}         - Remove produto
POST   /api/produto/EnviarArquivo - Upload de imagem
```

### Usuários
```
GET    /api/usuario              - Lista usuários
POST   /api/usuario              - Cria usuário
POST   /api/usuario/VerificarUsuario - Login
```

### Pedidos
```
POST   /api/pedido               - Finaliza pedido
```

### Swagger
```
GET    /swagger                  - Documentação interativa
```

## 🔒 Segurança

- Senhas armazenadas sem hash (⚠️ **Não use em produção!**)
- CORS configurado para desenvolvimento
- Validação de dados no backend e frontend
- Upload de arquivos com validação de extensão

## 🎨 Features Implementadas

✅ Cadastro e autenticação de usuários  
✅ CRUD completo de produtos  
✅ Upload de imagens de produtos  
✅ Carrinho de compras com persistência local  
✅ Incremento automático de quantidade no carrinho  
✅ Cálculo dinâmico de totais  
✅ Finalização de pedidos  
✅ Notificações toast  
✅ Responsive design com Bootstrap 5  
✅ Hot Module Replacement (HMR)  
✅ Documentação Swagger  
✅ **Testes unitários com 54 testes (100% aprovação)**  
✅ **Cobertura de código (~80-85% do código testável)**  

## 🛠️ Melhorias Futuras

- [ ] Implementar autenticação JWT
- [ ] Hash de senhas com bcrypt
- [ ] Paginação de produtos
- [ ] Filtros avançados de pesquisa
- [ ] Histórico de pedidos
- [ ] Painel administrativo completo
- [ ] Testes de integração e E2E
- [ ] CI/CD pipeline
- [ ] Deploy em Azure/AWS

## 🤝 Contribuição

Contribuições são bem-vindas! Para contribuir:

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/MinhaFeature`)
3. Commit suas mudanças (`git commit -m 'Adiciona MinhaFeature'`)
4. Push para a branch (`git push origin feature/MinhaFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo LICENSE para mais detalhes.

## 👤 Autor

**Emerson Bilieri**
- GitHub: [@ebilieri](https://github.com/ebilieri)

---

⭐ Se este projeto foi útil, considere dar uma estrela!

