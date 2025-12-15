# GroceryShop.QuickBuy - Documentação Técnica

## Sumário Executivo

Este documento fornece uma visão técnica detalhada da aplicação GroceryShop.QuickBuy, uma solução fullstack de e-commerce desenvolvida com ASP.NET Core 10.0 e Angular 19.0.6.

---

## 1. Visão Geral da Arquitetura

### 1.1 Arquitetura Geral

A aplicação segue os princípios de Clean Architecture com separação em três camadas principais:

```
┌─────────────────────────────────────────────────────────┐
│                    Presentation Layer                    │
│  (GroceryShop.Angular - Controllers + Angular SPA)      │
└───────────────────┬─────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────────────────┐
│                     Domain Layer                         │
│         (GroceryShop.Dominio - Entities + Contracts)    │
└───────────────────┬─────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────────────────┐
│                   Data Access Layer                      │
│    (GroceryShop.Repositorio - EF Core + Repositories)   │
└───────────────────┬─────────────────────────────────────┘
                    │
                    ▼
              ┌─────────┐
              │  MySQL  │
              └─────────┘
```

### 1.2 Tecnologias por Camada

#### Backend Stack
| Camada | Tecnologia | Versão | Propósito |
|--------|-----------|--------|-----------|
| Framework | .NET | 10.0 | Runtime principal |
| Web Framework | ASP.NET Core | 10.0 | APIs RESTful |
| ORM | Entity Framework Core | 9.0.1 | Mapeamento objeto-relacional |
| Banco de Dados | MySQL | 8.0+ | Persistência de dados |
| Serialização | Newtonsoft.Json | 10.0.1 | JSON handling |
| Documentação | Swagger/OpenAPI | 7.2.0 | Documentação de API |

#### Frontend Stack
| Componente | Tecnologia | Versão | Propósito |
|------------|-----------|--------|-----------|
| Framework | Angular | 19.0.6 | SPA framework |
| Linguagem | TypeScript | 5.6.3 | Type-safe JavaScript |
| UI Framework | Bootstrap | 5.3.3 | CSS framework |
| Notificações | ngx-toastr | 19.0.0 | Toast notifications |
| Estado Reativo | RxJS | 7.8.1 | Programação reativa |
| Build Tool | Angular CLI | 19.0.6 | Build e dev server |

---

## 2. Camada de Domínio (GroceryShop.Dominio)

### 2.1 Entidades

#### 2.1.1 Entidade Base
```csharp
public abstract class Entidade
{
    public int Id { get; set; }
    
    public virtual bool EhValido()
    {
        throw new NotImplementedException();
    }
}
```

#### 2.1.2 Produto
```csharp
public class Produto : Entidade
{
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public decimal Preco { get; set; }
    public string NomeArquivo { get; set; }
}
```

**Regras de Negócio:**
- Nome obrigatório
- Preço deve ser maior que zero
- Descrição opcional

#### 2.1.3 Usuario
```csharp
public class Usuario : Entidade
{
    public string Email { get; set; }
    public string Senha { get; set; }
    public string Nome { get; set; }
    public string SobreNome { get; set; }
    public ICollection<Pedido> Pedidos { get; set; }
}
```

**Regras de Negócio:**
- Email único e válido
- Senha mínima (implementação pendente)
- Nome e sobrenome obrigatórios

#### 2.1.4 Pedido
```csharp
public class Pedido : Entidade
{
    public DateTime DataPedido { get; set; }
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }
    public int FormaPagamentoId { get; set; }
    public FormaPagamento FormaPagamento { get; set; }
    public ICollection<ItemPedido> ItensPedido { get; set; }
}
```

#### 2.1.5 ItemPedido
```csharp
public class ItemPedido : Entidade
{
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public int PedidoId { get; set; }
    public Pedido Pedido { get; set; }
}
```

### 2.2 Value Objects

#### FormaPagamento
```csharp
public class FormaPagamento
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
}
```

**Formas de Pagamento Suportadas:**
1. Boleto
2. Cartão de Crédito
3. Depósito

### 2.3 Contratos (Interfaces)

#### IBaseRepositorio<T>
```csharp
public interface IBaseRepositorio<T> where T : class
{
    void Adicionar(T entidade);
    void Atualizar(T entidade);
    void Remover(T entidade);
    T ObterPorId(int id);
    IEnumerable<T> ObterTodos();
}
```

#### Interfaces Especializadas
- `IProdutoRepositorio : IBaseRepositorio<Produto>`
- `IUsuarioRepositorio : IBaseRepositorio<Usuario>`
- `IPedidoRepositorio : IBaseRepositorio<Pedido>`

---

## 3. Camada de Dados (GroceryShop.Repositorio)

### 3.1 Contexto do Banco de Dados

#### QuickBuyContexto
```csharp
public class QuickBuyContexto : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<ItemPedido> ItensPedidos { get; set; }
    public DbSet<FormaPagamento> FormaPagamentos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Fluent API configurations
        modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
        modelBuilder.ApplyConfiguration(new ProdutoConfiguration());
        modelBuilder.ApplyConfiguration(new PedidoConfiguration());
        modelBuilder.ApplyConfiguration(new ItemPedidoConfiguration());
        modelBuilder.ApplyConfiguration(new FormaPagamentoConfiguration());
    }
}
```

### 3.2 Configurações Entity Framework

#### ProdutoConfiguration
```csharp
public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Nome)
               .IsRequired()
               .HasMaxLength(100);
        builder.Property(p => p.Descricao)
               .HasMaxLength(500);
        builder.Property(p => p.Preco)
               .HasColumnType("decimal(18,2)")
               .IsRequired();
    }
}
```

### 3.3 Repositórios

#### BaseRepositorio<T>
```csharp
public abstract class BaseRepositorio<T> : IBaseRepositorio<T> 
    where T : class
{
    protected readonly QuickBuyContexto _contexto;

    public BaseRepositorio(QuickBuyContexto contexto)
    {
        _contexto = contexto;
    }

    public void Adicionar(T entidade)
    {
        _contexto.Set<T>().Add(entidade);
        _contexto.SaveChanges();
    }

    public IEnumerable<T> ObterTodos()
    {
        return _contexto.Set<T>().ToList();
    }
    
    // ... outros métodos
}
```

### 3.4 Schema do Banco de Dados

#### Tabela: Produtos
```sql
CREATE TABLE Produtos (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Nome VARCHAR(100) NOT NULL,
    Descricao VARCHAR(500),
    Preco DECIMAL(18,2) NOT NULL,
    NomeArquivo VARCHAR(255)
);
```

#### Tabela: Usuarios
```sql
CREATE TABLE Usuarios (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Senha VARCHAR(255) NOT NULL,
    Nome VARCHAR(100) NOT NULL,
    SobreNome VARCHAR(100) NOT NULL
);
```

#### Tabela: Pedidos
```sql
CREATE TABLE Pedidos (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    DataPedido DATETIME NOT NULL,
    UsuarioId INT NOT NULL,
    FormaPagamentoId INT NOT NULL,
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id),
    FOREIGN KEY (FormaPagamentoId) REFERENCES FormaPagamentos(Id)
);
```

#### Tabela: ItensPedidos
```sql
CREATE TABLE ItensPedidos (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    ProdutoId INT NOT NULL,
    Quantidade INT NOT NULL,
    PedidoId INT NOT NULL,
    FOREIGN KEY (ProdutoId) REFERENCES Produtos(Id),
    FOREIGN KEY (PedidoId) REFERENCES Pedidos(Id)
);
```

---

## 4. Camada de Apresentação - Backend (GroceryShop.Angular)

### 4.1 Configuração do Startup

#### Injeção de Dependências
```csharp
public void ConfigureServices(IServiceCollection services)
{
    // Database
    services.AddDbContext<QuickBuyContexto>(options =>
        options.UseMySql(
            Configuration.GetConnectionString("QuickByConnection"),
            ServerVersion.AutoDetect(Configuration.GetConnectionString("QuickByConnection"))
        )
    );

    // Repositories
    services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>();
    services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
    services.AddScoped<IPedidoRepositorio, PedidoRepositorio>();

    // Controllers
    services.AddControllersWithViews()
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = 
                    ReferenceLoopHandling.Ignore
            );

    // CORS
    services.AddCors(options =>
    {
        options.AddPolicy("AllowAll",
            builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
        );
    });

    // Swagger
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Grocery Shop API",
            Version = "v1",
            Description = "E-commerce REST API"
        });
    });

    // SPA
    services.AddSpaStaticFiles(configuration =>
    {
        configuration.RootPath = "ClientApp/dist/GroceryShop.Angular";
    });
}
```

### 4.2 Controllers

#### ProdutoController
```csharp
[Route("api/[controller]")]
[ApiController]
public class ProdutoController : ControllerBase
{
    private readonly IProdutoRepositorio _repositorio;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _hostingEnvironment;

    [HttpGet]
    public ActionResult<IEnumerable<Produto>> Get()
    {
        try
        {
            var produtos = _repositorio.ObterTodos();
            return Ok(produtos);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public ActionResult<Produto> Post([FromBody] Produto produto)
    {
        try
        {
            _repositorio.Adicionar(produto);
            return Created("", produto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("EnviarArquivo")]
    public ActionResult EnviarArquivo()
    {
        try
        {
            var formFile = _httpContextAccessor
                .HttpContext.Request.Form.Files["arquivo"];
            
            var nomeArquivo = formFile.FileName;
            var caminhoArquivo = Path.Combine(
                _hostingEnvironment.WebRootPath,
                "arquivos",
                nomeArquivo
            );

            using (var stream = new FileStream(
                caminhoArquivo, FileMode.Create))
            {
                formFile.CopyTo(stream);
            }

            return Ok(new { nomeArquivo });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
```

#### UsuarioController
```csharp
[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioRepositorio _repositorio;

    [HttpPost]
    public ActionResult<Usuario> Post([FromBody] Usuario usuario)
    {
        try
        {
            _repositorio.Adicionar(usuario);
            return Created("", usuario);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("VerificarUsuario")]
    public ActionResult<Usuario> VerificarUsuario(
        [FromBody] Usuario usuario)
    {
        try
        {
            var usuarioRetorno = _repositorio.Obter(
                usuario.Email,
                usuario.Senha
            );

            if (usuarioRetorno == null)
                return NotFound("Usuário não encontrado");

            return Ok(usuarioRetorno);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
```

### 4.3 Endpoints da API

#### Produtos

| Método | Endpoint | Descrição | Request Body | Response |
|--------|----------|-----------|--------------|----------|
| GET | `/api/produto` | Lista todos os produtos | - | `Produto[]` |
| POST | `/api/produto` | Cria novo produto | `Produto` | `Produto` |
| DELETE | `/api/produto/{id}` | Remove produto | - | `void` |
| POST | `/api/produto/EnviarArquivo` | Upload de imagem | `multipart/form-data` | `{ nomeArquivo }` |

#### Usuários

| Método | Endpoint | Descrição | Request Body | Response |
|--------|----------|-----------|--------------|----------|
| GET | `/api/usuario` | Lista todos os usuários | - | `Usuario[]` |
| POST | `/api/usuario` | Cadastra usuário | `Usuario` | `Usuario` |
| POST | `/api/usuario/VerificarUsuario` | Autentica usuário | `Usuario` | `Usuario` |

#### Pedidos

| Método | Endpoint | Descrição | Request Body | Response |
|--------|----------|-----------|--------------|----------|
| POST | `/api/pedido` | Finaliza pedido | `Pedido` | `Pedido` |

---

## 5. Camada de Apresentação - Frontend (Angular)

### 5.1 Arquitetura Angular

#### Estrutura de Módulos
```
AppModule (root)
├── LojaModule
│   ├── LojaPesquisaComponent
│   ├── LojaProdutoComponent
│   ├── LojaEfetivarComponent
│   └── LojaCompraFinalizadaComponent
├── ProdutoModule
│   ├── ProdutoComponent
│   └── PesquisaProdutoComponent
└── UsuarioModule
    ├── LoginComponent
    └── CadastroUsuarioComponent
```

### 5.2 Modelos TypeScript

#### Produto
```typescript
export class Produto {
  id: number;
  nome: string;
  descricao: string;
  preco: number;
  precoOriginal?: number;
  nomeArquivo: string;
  quantidade?: number;
  arquivoSelecionado?: File;
}
```

#### Usuario
```typescript
export class Usuario {
  id: number;
  email: string;
  senha: string;
  nome: string;
  sobreNome: string;
}
```

#### Pedido
```typescript
export class Pedido {
  id: number;
  dataPedido: Date;
  usuarioId: number;
  formaPagamentoId: number;
  itensPedido: ItemPedido[];
}
```

### 5.3 Serviços

#### ProdutoServico
```typescript
@Injectable({
  providedIn: 'root'
})
export class ProdutoServico extends BaseServico {
  private baseURL: string = `${this.UrlServiceV1}produto`;

  obterTodosProdutos(): Observable<Produto[]> {
    return this.http
      .get<Produto[]>(this.baseURL)
      .pipe(catchError(this.serviceError));
  }

  cadastrar(produto: Produto): Observable<Produto> {
    return this.http
      .post<Produto>(this.baseURL, produto, this.ObterHeaderJson())
      .pipe(catchError(this.serviceError));
  }

  enviarArquivo(arquivoSelecionado: File): Observable<string> {
    const formData: FormData = new FormData();
    formData.append('arquivo', arquivoSelecionado, 
                    arquivoSelecionado.name);

    return this.http
      .post<string>(`${this.baseURL}/EnviarArquivo`, formData)
      .pipe(catchError(this.serviceError));
  }
}
```

#### UsuarioServico
```typescript
@Injectable({
  providedIn: 'root'
})
export class UsuarioServico extends BaseServico {
  private baseURL: string = `${this.UrlServiceV1}usuario`;

  verificarUsuario(usuario: Usuario): Observable<Usuario> {
    return this.http
      .post<Usuario>(`${this.baseURL}/VerificarUsuario`, 
                     usuario, 
                     this.ObterHeaderJson())
      .pipe(catchError(this.serviceError));
  }

  cadastrar(usuario: Usuario): Observable<Usuario> {
    return this.http
      .post<Usuario>(this.baseURL, usuario, this.ObterHeaderJson())
      .pipe(catchError(this.serviceError));
  }

  get usuario(): Usuario {
    const usuario_json = sessionStorage.getItem('usuario-autenticado');
    return JSON.parse(usuario_json);
  }

  limparSessao() {
    sessionStorage.setItem('usuario-autenticado', '');
  }
}
```

### 5.4 Gestão de Estado - Carrinho de Compras

#### LojaCarrinhoCompras (Singleton)
```typescript
export class LojaCarrinhoCompras {
  private itens: Produto[] = [];

  constructor() {
    this.carregarLocalStorage();
  }

  public adicionar(produto: Produto): void {
    const produtoLocalStorage = this.itens.find(
      p => p.id === produto.id
    );

    if (produtoLocalStorage) {
      produtoLocalStorage.quantidade = 
        (produtoLocalStorage.quantidade || 0) + 1;
    } else {
      produto.quantidade = 1;
      this.itens.push(produto);
    }

    this.salvarLocalStorage();
  }

  public removerProduto(produto: Produto): void {
    this.itens = this.itens.filter(p => p.id !== produto.id);
    this.salvarLocalStorage();
  }

  public obterProdutos(): Produto[] {
    this.carregarLocalStorage();
    return this.itens;
  }

  public limpar(): void {
    this.itens = [];
    localStorage.removeItem('shopping_cart');
  }

  private salvarLocalStorage(): void {
    localStorage.setItem('shopping_cart', 
                        JSON.stringify(this.itens));
  }

  private carregarLocalStorage(): void {
    const itens = localStorage.getItem('shopping_cart');
    this.itens = itens ? JSON.parse(itens) : [];
  }
}
```

### 5.5 Componentes Principais

#### LojaPesquisaComponent
```typescript
@Component({
  standalone: false,
  selector: 'app-loja-pesquisa',
  templateUrl: './loja.pesquisa.component.html'
})
export class LojaPesquisaComponent implements OnInit {
  public produtos: Produto[];

  constructor(private produtoServico: ProdutoServico) {}

  ngOnInit(): void {
    this.produtoServico.obterTodosProdutos()
      .subscribe(
        produtos => this.produtos = produtos,
        error => console.error(error)
      );
  }
}
```

#### LojaEfetivarComponent (Carrinho)
```typescript
@Component({
  standalone: false,
  selector: 'loja-efetivar',
  templateUrl: './loja.efetivar.component.html'
})
export class LojaEfetivarComponent implements OnInit {
  public produtos: Produto[] = [];
  public total: number = 0;
  public carrinhoCompras: LojaCarrinhoCompras;

  ngOnInit(): void {
    this.carrinhoCompras = new LojaCarrinhoCompras();
    this.produtos = this.carrinhoCompras.obterProdutos();
    this.atualizarTotal();
  }

  public atualizarTotal(): void {
    this.total = this.produtos.reduce((acc, produto) => {
      const precoUnitario = produto.precoOriginal || produto.preco;
      const quantidade = produto.quantidade || 1;
      return acc + (precoUnitario * quantidade);
    }, 0);
  }

  public remover(produto: Produto): void {
    this.carrinhoCompras.removerProduto(produto);
    this.produtos = this.carrinhoCompras.obterProdutos();
    this.atualizarTotal();
  }
}
```

### 5.6 Roteamento

#### app-routing.module.ts
```typescript
const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'loja-pesquisa', component: LojaPesquisaComponent },
  { path: 'loja-produto/:idProduto', component: LojaProdutoComponent },
  { path: 'loja-efetivar', component: LojaEfetivarComponent },
  { 
    path: 'loja-compra-finalizada', 
    component: LojaCompraFinalizadaComponent 
  },
  { path: 'produto', component: ProdutoComponent },
  { path: 'pesquisar-produto', component: PesquisaProdutoComponent },
  { path: 'entrar', component: LoginComponent },
  { path: 'novo-usuario', component: CadastroUsuarioComponent },
];
```

### 5.7 Configuração de Build

#### angular.json (Production Build)
```json
{
  "production": {
    "optimization": true,
    "outputHashing": "all",
    "sourceMap": false,
    "namedChunks": false,
    "extractLicenses": true,
    "vendorChunk": false,
    "buildOptimizer": true,
    "budgets": [
      {
        "type": "initial",
        "maximumWarning": "2mb",
        "maximumError": "5mb"
      }
    ]
  }
}
```

---

## 6. Fluxos de Dados

### 6.1 Fluxo de Cadastro de Produto

```
┌──────────────┐      ┌──────────────────┐      ┌─────────────────┐
│  Componente  │─────>│  ProdutoServico  │─────>│ ProdutoController│
│  (Angular)   │      │  (HTTP Client)   │      │  (API)          │
└──────────────┘      └──────────────────┘      └────────┬────────┘
                                                           │
                                                           ▼
                                                  ┌────────────────┐
                                                  │  Repositorio   │
                                                  └────────┬───────┘
                                                           │
                                                           ▼
                                                  ┌────────────────┐
                                                  │  EF Core       │
                                                  └────────┬───────┘
                                                           │
                                                           ▼
                                                  ┌────────────────┐
                                                  │    MySQL       │
                                                  └────────────────┘
```

### 6.2 Fluxo de Autenticação

```
1. Usuário preenche email/senha no LoginComponent
2. LoginComponent chama UsuarioServico.verificarUsuario()
3. UsuarioServico faz POST /api/usuario/VerificarUsuario
4. UsuarioController consulta UsuarioRepositorio
5. Se válido: armazena usuário em sessionStorage
6. Redireciona para página de produtos
```

### 6.3 Fluxo de Carrinho de Compras

```
┌────────────────────────────────────────────────────┐
│ 1. Usuário clica "Adicionar ao Carrinho"          │
└────────────────────┬───────────────────────────────┘
                     ▼
┌────────────────────────────────────────────────────┐
│ 2. LojaCarrinhoCompras.adicionar(produto)          │
│    - Verifica se produto já existe                 │
│    - Se existe: incrementa quantidade              │
│    - Se não: adiciona com quantidade = 1           │
└────────────────────┬───────────────────────────────┘
                     ▼
┌────────────────────────────────────────────────────┐
│ 3. Salva no localStorage ('shopping_cart')         │
└────────────────────┬───────────────────────────────┘
                     ▼
┌────────────────────────────────────────────────────┐
│ 4. LojaEfetivarComponent carrega do localStorage   │
│    - Lê produtos                                   │
│    - Calcula total: Σ(preço × quantidade)          │
└────────────────────────────────────────────────────┘
```

### 6.4 Fluxo de Finalização de Pedido

```
1. LojaEfetivarComponent.efetivarCompra()
2. Cria objeto Pedido com:
   - usuarioId (da sessão)
   - formaPagamentoId (selecionada)
   - itensPedido (do carrinho)
3. PedidoServico.cadastrar(pedido)
4. POST /api/pedido
5. PedidoController salva no banco
6. Retorna pedidoId
7. Armazena pedidoId em sessionStorage
8. Limpa carrinho (localStorage)
9. Redireciona para loja-compra-finalizada
```

---

## 7. Persistência de Dados

### 7.1 LocalStorage (Frontend)

#### Dados Armazenados
```typescript
// Carrinho de compras
localStorage.setItem('shopping_cart', JSON.stringify(produtos));

// Estrutura:
{
  "shopping_cart": [
    {
      "id": 1,
      "nome": "Produto X",
      "preco": 10.50,
      "precoOriginal": 10.50,
      "quantidade": 2,
      "nomeArquivo": "produto1.jpg"
    }
  ]
}
```

### 7.2 SessionStorage (Frontend)

```typescript
// Usuário autenticado
sessionStorage.setItem('usuario-autenticado', JSON.stringify(usuario));

// ID do pedido finalizado
sessionStorage.setItem('pedidoId', pedidoId.toString());
```

### 7.3 Banco de Dados (Backend)

#### String de Conexão
```json
{
  "ConnectionStrings": {
    "QuickByConnection": "server=localhost;userid=root;password=root;database=QuickBuyAngularDB"
  }
}
```

#### Migrações
```bash
# Criar migração
dotnet ef migrations add NomeMigracao

# Aplicar migração
dotnet ef database update

# Reverter migração
dotnet ef database update PreviousMigrationName
```

---

## 8. Segurança

### 8.1 Vulnerabilidades Conhecidas

⚠️ **ATENÇÃO: Esta aplicação possui vulnerabilidades de segurança e não deve ser usada em produção sem as correções abaixo:**

#### 8.1.1 Senhas em Texto Plano
```csharp
// ATUAL (INSEGURO)
public Usuario Obter(string email, string senha)
{
    return _contexto.Usuarios
        .FirstOrDefault(u => u.Email == email && u.Senha == senha);
}

// RECOMENDADO
public Usuario Obter(string email, string senha)
{
    var usuario = _contexto.Usuarios
        .FirstOrDefault(u => u.Email == email);
    
    if (usuario != null && BCrypt.Verify(senha, usuario.SenhaHash))
        return usuario;
    
    return null;
}
```

#### 8.1.2 Ausência de Autenticação JWT
```csharp
// RECOMENDADO: Adicionar JWT
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Configuration["Jwt:Issuer"],
            ValidAudience = Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
        };
    });
```

#### 8.1.3 CORS Aberto
```csharp
// ATUAL (INSEGURO)
services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                         .AllowAnyMethod()
                         .AllowAnyHeader()
    );
});

// RECOMENDADO
services.AddCors(options =>
{
    options.AddPolicy("Production",
        builder => builder.WithOrigins("https://seudominio.com")
                         .AllowAnyMethod()
                         .AllowAnyHeader()
    );
});
```

### 8.2 Validação de Upload de Arquivos

```csharp
// Implementar validações
private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
private const long _maxFileSize = 5 * 1024 * 1024; // 5MB

public ActionResult EnviarArquivo()
{
    var formFile = _httpContextAccessor.HttpContext.Request.Form.Files["arquivo"];
    
    // Validar extensão
    var extension = Path.GetExtension(formFile.FileName).ToLowerInvariant();
    if (!_allowedExtensions.Contains(extension))
        return BadRequest("Tipo de arquivo não permitido");
    
    // Validar tamanho
    if (formFile.Length > _maxFileSize)
        return BadRequest("Arquivo muito grande");
    
    // Gerar nome único
    var nomeArquivo = $"{Guid.NewGuid()}{extension}";
    
    // ... resto do código
}
```

---

## 9. Performance e Otimizações

### 9.1 Backend

#### Entity Framework
```csharp
// Usar AsNoTracking para queries read-only
public IEnumerable<Produto> ObterTodos()
{
    return _contexto.Produtos
        .AsNoTracking()
        .ToList();
}

// Eager Loading para evitar N+1
public Pedido ObterPorId(int id)
{
    return _contexto.Pedidos
        .Include(p => p.ItensPedido)
            .ThenInclude(i => i.Produto)
        .Include(p => p.Usuario)
        .Include(p => p.FormaPagamento)
        .FirstOrDefault(p => p.Id == id);
}
```

#### Response Caching
```csharp
[HttpGet]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public ActionResult<IEnumerable<Produto>> Get()
{
    return Ok(_repositorio.ObterTodos());
}
```

### 9.2 Frontend

#### Lazy Loading de Módulos
```typescript
const routes: Routes = [
  {
    path: 'loja',
    loadChildren: () => import('./loja/loja.module')
      .then(m => m.LojaModule)
  },
  {
    path: 'admin',
    loadChildren: () => import('./produto/produto.module')
      .then(m => m.ProdutoModule)
  }
];
```

#### OnPush Change Detection
```typescript
@Component({
  standalone: false,
  selector: 'app-produto-list',
  templateUrl: './produto-list.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProdutoListComponent {}
```

#### TrackBy em *ngFor
```typescript
trackByProdutoId(index: number, produto: Produto): number {
  return produto.id;
}
```

```html
<div *ngFor="let produto of produtos; trackBy: trackByProdutoId">
  {{ produto.nome }}
</div>
```

---

## 10. Testes

### 10.1 Testes Unitários (Recomendado)

#### Backend - xUnit
```csharp
public class ProdutoRepositorioTests
{
    [Fact]
    public void Adicionar_DeveSalvarProdutoNoBanco()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<QuickBuyContexto>()
            .UseInMemoryDatabase(databaseName: "TestDB")
            .Options;
        
        using (var context = new QuickBuyContexto(options))
        {
            var repositorio = new ProdutoRepositorio(context);
            var produto = new Produto
            {
                Nome = "Teste",
                Preco = 10.50m
            };

            // Act
            repositorio.Adicionar(produto);

            // Assert
            Assert.Equal(1, context.Produtos.Count());
        }
    }
}
```

#### Frontend - Jasmine/Karma
```typescript
describe('ProdutoServico', () => {
  let service: ProdutoServico;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ProdutoServico]
    });

    service = TestBed.inject(ProdutoServico);
    httpMock = TestBed.inject(HttpTestingController);
  });

  it('deve obter todos os produtos', () => {
    const mockProdutos: Produto[] = [
      { id: 1, nome: 'Produto 1', preco: 10 }
    ];

    service.obterTodosProdutos().subscribe(produtos => {
      expect(produtos.length).toBe(1);
      expect(produtos).toEqual(mockProdutos);
    });

    const req = httpMock.expectOne(`${service.baseURL}`);
    expect(req.request.method).toBe('GET');
    req.flush(mockProdutos);
  });
});
```

---

## 11. Deploy

### 11.1 Build de Produção

#### Backend
```bash
cd GroceryShop.Angular
dotnet publish -c Release -o ./publish
```

#### Frontend
```bash
cd GroceryShop.Angular/ClientApp
npm run build --prod
```

### 11.2 IIS (Windows)

1. Instalar .NET Hosting Bundle
2. Criar Application Pool (.NET CLR Version: No Managed Code)
3. Criar Website apontando para pasta publish
4. Configurar web.config:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <system.webServer>
    <handlers>
      <add name="aspNetCore" path="*" verb="*" 
           modules="AspNetCoreModuleV2" 
           resourceType="Unspecified" />
    </handlers>
    <aspNetCore processPath="dotnet" 
                arguments=".\GroceryShop.Angular.dll" 
                stdoutLogEnabled="false" 
                stdoutLogFile=".\logs\stdout" 
                hostingModel="inprocess" />
  </system.webServer>
</configuration>
```

### 11.3 Docker (Recomendado)

#### Dockerfile
```dockerfile
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY ["GroceryShop.Angular/GroceryShop.Angular.csproj", "GroceryShop.Angular/"]
COPY ["GroceryShop.Dominio/GroceryShop.Dominio.csproj", "GroceryShop.Dominio/"]
COPY ["GroceryShop.Repositorio/GroceryShop.Repositorio.csproj", "GroceryShop.Repositorio/"]
RUN dotnet restore "GroceryShop.Angular/GroceryShop.Angular.csproj"

# Copy everything and build
COPY . .
WORKDIR "/src/GroceryShop.Angular"
RUN dotnet build "GroceryShop.Angular.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "GroceryShop.Angular.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GroceryShop.Angular.dll"]
```

#### docker-compose.yml
```yaml
version: '3.8'

services:
  web:
    build: .
    ports:
      - "5000:80"
      - "5001:443"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__QuickByConnection=server=db;userid=root;password=root;database=QuickBuyAngularDB
    depends_on:
      - db

  db:
    image: mysql:8.0
    environment:
      MYSQL_ROOT_PASSWORD: root
      MYSQL_DATABASE: QuickBuyAngularDB
    ports:
      - "3306:3306"
    volumes:
      - mysql_data:/var/lib/mysql

volumes:
  mysql_data:
```

---

## 12. Monitoramento e Logging

### 12.1 Logging (Recomendado)

#### Serilog
```csharp
// Program.cs
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseSerilog((context, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", 
                              rollingInterval: RollingInterval.Day);
        })
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
```

### 12.2 Health Checks
```csharp
services.AddHealthChecks()
    .AddDbContextCheck<QuickBuyContexto>();

app.UseHealthChecks("/health");
```

---

## 13. Referências

### Documentação Oficial
- [ASP.NET Core](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [Angular](https://angular.io/docs)
- [TypeScript](https://www.typescriptlang.org/docs)
- [Bootstrap](https://getbootstrap.com/docs)

### Repositórios
- GitHub: https://github.com/ebilieri/GroceryShop.QuickBuy.New

---

**Versão:** 1.0  
**Data:** Dezembro 2025  
**Autor:** Emerson Bilieri
