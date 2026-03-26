# Estrutura de Pastas — Ordin

Este arquivo descreve a estrutura de pastas do projeto Ordin.

```
Ordin.sln
Ordin.slnx
Ordin.sln.DotSettings.user

Ordin.Api/
├─ Ordin.Api.csproj
├─ Program.cs
├─ appsettings.json
├─ appsettings.Development.json
├─ Ordin.Api.http
├─ Controllers/
│  └─ TransactionsController.cs
├─ Properties/
│  └─ launchSettings.json
├─ bin/
└─ obj/

Ordin.Application/
├─ Ordin.Application.csproj
├─ Interfaces/
│  ├─ IBaseRepository.cs
│  └─ ICurrentUserService.cs
├─ bin/
└─ obj/

Ordin.Domain/
├─ Ordin.Domain.csproj
├─ Entities/
│  ├─ BaseEntity.cs
│  ├─ Category.cs
│  ├─ Transaction.cs
│  └─ User.cs
├─ Enums/
│  └─ TransactionType.cs
├─ ValueObjects/
│  └─ Money.cs
├─ bin/
└─ obj/

Ordin.Infra/
├─ Ordin.Infra.csproj
├─ UnitOfWork.cs
├─ Configurations/
│  ├─ BaseEntityConfiguration.cs
│  ├─ CategoryConfiguration.cs
│  ├─ TransactionConfiguration.cs
│  └─ UserConfiguration.cs
├─ Contexts/
│  └─ OrdinContext.cs
├─ Extensions/
│  └─ ModelBuilderExtensions.cs
├─ Repositories/
│  └─ BaseRepository.cs
├─ Services/
│  └─ UserService.cs
├─ bin/
└─ obj/
```