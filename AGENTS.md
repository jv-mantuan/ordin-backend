# AGENTS.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Restore dependencies
dotnet restore

# Run the API
dotnet run --project Ordin.Api

# Build all projects
dotnet build

# EF Core migrations
dotnet ef migrations add <MigrationName> --project Ordin.Infra --startup-project Ordin.Api
dotnet ef database update --project Ordin.Infra --startup-project Ordin.Api
dotnet ef migrations remove --project Ordin.Infra --startup-project Ordin.Api
```

No test projects exist yet.

## Architecture

Four-layer **Clean Architecture** with **CQRS** and **DDD-inspired** domain modeling:

- **`Ordin.Domain`** — Pure domain layer, no external dependencies. Entities extend `BaseEntity` (Guid Id, DateTimeOffset CreatedAt/UpdatedAt). `Money` is a value object enforcing non-negative amounts. All entities use soft deletes (`IsDeleted`).
- **`Ordin.Application`** — CQRS handlers, interfaces, DTOs. Commands implement `ICommand<TResult>`, queries implement `IQuery<TResult>`. All handlers return `ErrorOr<TResult>`. Handlers are auto-discovered via reflection in `HandlerRegistrationExtensions`.
- **`Ordin.Infra`** — EF Core 10 + Npgsql (PostgreSQL). Snake_case naming via `EFCore.NamingConventions`. `OrdinContext.SaveChangesAsync` auto-updates timestamps. Generic `BaseRepository<T>` extended by specialized repositories.
- **`Ordin.Api`** — ASP.NET Core controllers at route prefix `v1/[controller]`. `BaseController` handles `ErrorOr` → HTTP status mapping and wraps all responses in `ApiResponse<T>` (includes RequestId from `Activity.Current?.Id`).

## Key Patterns

**Adding a new feature** follows this flow:
1. Domain entity in `Ordin.Domain/Entities/` extending `BaseEntity`
2. EF config in `Ordin.Infra/Configurations/` (applied automatically via `ApplyConfigurationsFromAssembly`)
3. Repository interface in `Ordin.Application/Interfaces/`, implementation in `Ordin.Infra/Repositories/` (registered automatically via reflection)
4. Command/Query records + Handler classes in `Ordin.Application/Commands/` or `Ordin.Application/Queries/` (registered automatically)
5. Controller in `Ordin.Api/Controllers/` extending `BaseController`

**CQRS dispatch**: Controllers call `await _dispatcher.CommandAsync(new XCommand(...), ct)` or `await _dispatcher.QueryAsync(new XQuery(...), ct)`. All async methods use `CancellationToken ct` as parameter name.

**Commands/Queries are records**: `record CreateXCommand(...) : ICommand<XDto>` and `record GetXQuery(...) : IQuery<XDto>`.

**Current user**: `ICurrentUserService.UserId` is injected into handlers. Currently returns a hardcoded Guid (auth not yet implemented).

**Error handling**: Return `Error.NotFound()`, `Error.Conflict()`, etc. from handlers. `BaseController.Problem(Error error)` maps them to HTTP 404, 409, 400, 401, 403, 500.

**Entity conventions**:
- Properties use `private set` — state is only mutated via entity methods (`Update(...)`, `Delete()`).
- `Delete()` sets `IsDeleted = true` (soft delete). Repositories must filter out deleted records.
- Use a static factory method `Entity.Create(...)` when creation involves business logic (see `Category`); otherwise a public constructor is fine.
- All entities need a private parameterless constructor for EF Core.

**Value objects**: `Money` stores only its `decimal Value` in the database via a custom EF conversion. Use `Money.ByPass(value)` exclusively in EF configurations to reconstruct without validation.

**EF configuration conventions**: Foreign keys use `OnDelete(DeleteBehavior.Restrict)` to prevent accidental cascade deletes.

**DTO naming**:
- Output DTOs (returned from handlers): `XDto` or `XWithYDto` — live in `Ordin.Application/DTOs/`.
- Request DTOs (received by the API): `XRequestDto` — live in `Ordin.Api/Contracts/`.

**Controller responses**:
- Successful creates return `CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)`.
- All successful responses are automatically wrapped in `ApiResponse<T>` by `BaseController.Ok()`.

**Serialization**: API responses use `camelCase` (JSON); database columns use `snake_case` (EF naming convention).

## Database

Default connection string (fallback in `Program.cs`): `Host=localhost;Database=ordin;Username=postgres;Password=3011`. Override via `appsettings.Development.json` or environment variable.

## Tech Stack

- .NET 10, ASP.NET Core
- EF Core 10 + Npgsql (PostgreSQL)
- `ErrorOr` 2.0.1 for result/error handling
- Swagger/OpenAPI via Swashbuckle (XML docs enabled)
