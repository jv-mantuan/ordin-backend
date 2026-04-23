using Ordin.Domain.Entities;
using Ordin.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ordin.Infra.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(transaction => transaction.Id);

        builder.Property(transaction => transaction.Name)
            .IsRequired();

        builder.Property(transaction => transaction.Type)
            .IsRequired();

        builder.Property(transaction => transaction.Date)
            .IsRequired();

        builder.HasOne(transaction => transaction.Category)
            .WithMany(category => category.Transactions)
            .HasForeignKey(transaction => transaction.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(transaction => transaction.User)
               .WithMany(u => u.Transactions)
               .HasForeignKey(fe => fe.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Property(transaction => transaction.Amount)
            .HasConversion(s => s.Value, s => Money.ByPass(s));

        builder.Property(transaction => transaction.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        builder.HasIndex(t => new { t.CategoryId, t.IsDeleted, t.Date });
        builder.HasIndex(t => new { t.UserId, t.IsDeleted, t.Date });
    }
}