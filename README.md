# Ordin

API de controle financeiro construída com .NET 10, EF Core e PostgreSQL.

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4)
![Status](https://img.shields.io/badge/status-em%20desenvolvimento-orange)
![Build](https://img.shields.io/badge/build-local-lightgrey)

## Stack

- .NET `net10.0`
- ASP.NET Core Web API
- Entity Framework Core + Npgsql
- Swagger (OpenAPI)

## Estrutura do Projeto

- `Ordin.Api`: camada de entrada HTTP, composição de DI e Swagger
- `Ordin.Application`: casos de uso, handlers, contratos e DTOs
- `Ordin.Domain`: entidades e regras de domínio
- `Ordin.Infra`: acesso a dados, contexto EF, repositórios e migrations

## Pré-requisitos

- .NET SDK 10
- PostgreSQL em execução
- `dotnet-ef` instalado globalmente

```bash
dotnet tool install --global dotnet-ef
```

## Configuração

A API usa a string de conexão abaixo como fallback no `Program.cs`:

`Host=localhost;Database=ordin;Username=postgres;Password=3011`

Para configurar por ambiente, adicione em `Ordin.Api/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=ordin;Username=postgres;Password=postgres"
  }
}
```

## Como Rodar

Na raiz da solução:

```bash
dotnet restore
dotnet run --project Ordin.Api
```

Swagger:

- `https://localhost:xxxx/` (rota raiz em ambiente de desenvolvimento)

## Migrations (EF Core)

As migrations ficam no módulo `Ordin.Infra`.

Adicionar migration:

```bash
dotnet ef migrations add NomeDaMigration --project Ordin.Infra --startup-project Ordin.Api --context OrdinContext --output-dir Migrations
```

Aplicar no banco:

```bash
dotnet ef database update --project Ordin.Infra --startup-project Ordin.Api --context OrdinContext
```

Remover última migration (não aplicada):

```bash
dotnet ef migrations remove --project Ordin.Infra --startup-project Ordin.Api --context OrdinContext
```

## DI

A composição de dependências ocorre na API (`Ordin.Api/Program.cs`):

- `AddApplicationHandlers()`
- `AddInfrastructureServices()`
- registros de serviços de aplicação (`IDispatcher`, `ICurrentUserService`, etc.)

## Endpoint Atual

- `GET /v1/Transactions`
  - Controller: `Ordin.Api/Controllers/TransactionsController.cs`
  - Retorno: lista de `TransactionDto`

## Notas

- Se o build falhar com arquivo `.dll` bloqueado (`MSB3021/MSB3027`), pare a API em execução e compile novamente.
- Se ocorrer `PendingModelChangesWarning` no EF, revise valores dinâmicos no mapeamento (ex.: defaults baseados em `UtcNow` no model).
