# Sistema de Gestão de Consultas UVV

Projeto do Trabalho Prático da disciplina de **Desenvolvimento Web Back-end**, desenvolvido com **C# + ASP.NET Core MVC + Entity Framework Core + SQL Server**.

## Funcionalidades

- Cadastro de usuário com validação por Data Annotations.
- Senha armazenada de forma segura usando hash (`PasswordHasher`).
- Login e logout com autenticação baseada em cookies.
- Proteção das rotas de consultas com `[Authorize]`.
- Cadastro, listagem, edição e exclusão de consultas.
- Cada usuário visualiza e manipula somente suas próprias consultas.
- Persistência Code First com EF Core e Migration inicial.
- Injeção de dependência do `AppDbContext` em `Program.cs`.
- Pipeline com `UseAuthentication()` antes de `UseAuthorization()`.

## Tecnologias

- .NET 8
- ASP.NET Core MVC
- Entity Framework Core 8
- SQL Server / LocalDB
- HTML5 + CSS3
- Data Annotations
- Cookie Authentication

## Pré-requisitos

1. .NET 8 SDK instalado.
2. SQL Server ou SQL Server LocalDB instalado.
3. Visual Studio 2022, Visual Studio Code ou outra IDE compatível.
4. Git instalado para publicação no GitHub.

## Configuração do banco

A conexão padrão está em `appsettings.json`:

```json
"DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=SistemaGestaoConsultasUVV;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
```

Se estiver usando uma instância diferente do SQL Server, altere somente a connection string.

### Pelo Visual Studio / Package Manager Console

Execute:

```powershell
Update-Database
```

### Pela CLI do .NET

Na pasta do projeto:

```bash
dotnet restore
dotnet tool install --global dotnet-ef --version 8.0.20
dotnet ef database update
dotnet run
```

> Se o `dotnet-ef` já estiver instalado, basta executar `dotnet ef database update`.

## Executando o projeto

```bash
dotnet restore
dotnet ef database update
dotnet run
```

Depois, acesse a URL apresentada no terminal, normalmente `https://localhost:7154` ou `http://localhost:5154`.

## Fluxo de demonstração

1. Acesse **Criar conta**.
2. Cadastre nome, e-mail e senha.
3. Faça login.
4. Entre em **Nova consulta**.
5. Informe especialidade, data/hora e descrição.
6. Salve e confirme a consulta na listagem.
7. Edite a consulta.
8. Exclua a consulta.
9. Faça logout.

## Arquitetura

```text
SistemaGestaoConsultasUVV/
├── Controllers/
│   ├── AccountController.cs
│   ├── ConsultasController.cs
│   └── HomeController.cs
├── Data/
│   └── AppDbContext.cs
├── Migrations/
├── Models/
│   ├── Usuario.cs
│   └── Consulta.cs
├── ViewModels/
│   ├── LoginViewModel.cs
│   └── RegistroUsuarioViewModel.cs
├── Views/
│   ├── Account/
│   ├── Consultas/
│   ├── Home/
│   └── Shared/
├── wwwroot/
├── Program.cs
├── appsettings.json
└── README.md
```

## Segurança

- `[Authorize]` protege o controller de consultas.
- `UseAuthentication()` está antes de `UseAuthorization()`.
- Formulários POST usam `ValidateAntiForgeryToken`.
- Senhas não são salvas em texto puro; são armazenadas como hash.
- O `UsuarioId` da consulta é obtido pelo `ClaimTypes.NameIdentifier` do usuário autenticado, evitando que o formulário escolha outro usuário.
- As operações de editar/excluir filtram pelo ID da consulta **e** pelo ID do usuário autenticado.
- O `returnUrl` do login somente é usado quando é uma URL local.

## Endpoints principais

| Método | Rota | Função |
|---|---|---|
| GET | `/Account/Register` | Tela de cadastro |
| POST | `/Account/Register` | Cria usuário |
| GET | `/Account/Login` | Tela de login |
| POST | `/Account/Login` | Autentica usuário |
| POST | `/Account/Logout` | Encerra sessão |
| GET | `/Consultas` | Lista consultas do usuário |
| GET | `/Consultas/Create` | Tela de nova consulta |
| POST | `/Consultas/Create` | Cria consulta |
| GET | `/Consultas/Edit/{id}` | Tela de edição |
| POST | `/Consultas/Edit/{id}` | Atualiza consulta |
| GET | `/Consultas/Delete/{id}` | Confirma exclusão |
| POST | `/Consultas/Delete/{id}` | Exclui consulta |

## Vídeo demonstrativo obrigatório

Substitua o endereço abaixo pelo vídeo gravado no Loom, YouTube ou serviço similar:

**[VÍDEO DEMONSTRATIVO - inserir link aqui](https://example.com/SEU-VIDEO-AQUI)**

O vídeo deve mostrar pelo menos: **cadastro, login e registro de consulta**. Para uma demonstração mais completa, mostre também edição, exclusão e logout.

## Entrega em PDF

No PDF do portal, informe os integrantes do grupo em ordem alfabética e o link do repositório GitHub.

- Integrantes: **PREENCHER**
- Repositório: **PREENCHER**
- Vídeo: **PREENCHER**

## Observação acadêmica

Este projeto foi estruturado para atender aos itens do enunciado: MVC/SoC, EF Core Code First, SQL Server, Data Annotations, DI, autenticação, autorização e CRUD de consultas.
