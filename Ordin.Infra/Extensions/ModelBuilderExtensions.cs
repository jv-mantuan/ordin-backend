using Ordin.Domain.Entities;
using Ordin.Domain.Enums;
using Ordin.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Ordin.Infra.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var adminId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var catAlimentacaoId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var catTransporteId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var deafaultDate = new DateTimeOffset(new DateTime(2026, 2, 12, 21, 21, 0));

            modelBuilder.Entity<User>().HasData(new
            {
                Id = adminId,
                Name = "Master",
                CreatedAt = deafaultDate,
                Email = "master@master.com",
                ExternalId = ""
            });

            modelBuilder.Entity<Category>().HasData(
                new { Id = catAlimentacaoId, Name = "Alimentação", UserId = adminId, CreatedAt = deafaultDate },
                new { Id = catTransporteId, Name = "Transporte", UserId = adminId, CreatedAt = deafaultDate }
            );

            modelBuilder.Entity<Transaction>().HasData(
                new
                {
                    Id = Guid.Parse("A1111111-1111-1111-1111-111111111111"),
                    Name = "Supermercado Continente",
                    Amount = (Money)55.50m,
                    Date = deafaultDate.AddDays(-2),
                    CategoryId = catAlimentacaoId,
                    UserId = adminId,
                    Type = TransactionType.Expense,
                    CreatedAt = deafaultDate
                },
                new
                {
                    Id = Guid.Parse("A2222222-2222-2222-2222-222222222222"),
                    Name = "Almoço de Trabalho",
                    Amount = (Money)15.00m,
                    Date = deafaultDate.AddDays(-1),
                    CategoryId = catAlimentacaoId,
                    UserId = adminId,
                    Type = TransactionType.Expense,
                    CreatedAt = deafaultDate
                },
                new
                {
                    Id = Guid.Parse("A3333333-3333-3333-3333-333333333333"),
                    Name = "Padaria",
                    Amount = (Money)4.20m,
                    Date = deafaultDate,
                    CategoryId = catAlimentacaoId,
                    UserId = adminId,
                    Type = TransactionType.Expense,
                    CreatedAt = deafaultDate
                },

                new
                {
                    Id = Guid.Parse("B1111111-1111-1111-1111-111111111111"),
                    Name = "Combustível Galp",
                    Amount = (Money)60.00m,
                    Date = deafaultDate.AddDays(-5),
                    CategoryId = catTransporteId,
                    UserId = adminId,
                    Type = TransactionType.Expense,
                    CreatedAt = deafaultDate
                },
                new
                {
                    Id = Guid.Parse("B2222222-2222-2222-2222-222222222222"),
                    Name = "Passe Mensal Metro",
                    Amount = (Money)30.00m,
                    Date = deafaultDate.AddDays(-10),
                    CategoryId = catTransporteId,
                    UserId = adminId,
                    Type = TransactionType.Expense,
                    CreatedAt = deafaultDate
                },
                new
                {
                    Id = Guid.Parse("B3333333-3333-3333-3333-333333333333"),
                    Name = "Uber para o Aeroporto",
                    Amount = (Money)12.50m,
                    Date = deafaultDate.AddDays(-3),
                    CategoryId = catTransporteId,
                    UserId = adminId,
                    Type = TransactionType.Expense,
                    CreatedAt = deafaultDate
                }
            );
        }
    }
}