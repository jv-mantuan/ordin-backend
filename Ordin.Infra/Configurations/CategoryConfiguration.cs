using Ordin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ordin.Infra.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(category => category.Id);

        builder.Property(category => category.Name)
            .IsRequired();

        builder.HasMany(category => category.Transactions)
            .WithOne(transaction => transaction.Category)
            .HasForeignKey(transaction => transaction.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(category => category.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        builder.HasIndex(category => new { category.Name, category.UserId });
    }
}
