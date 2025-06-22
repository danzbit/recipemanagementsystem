using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeManagementSystem.Domain.Entities;

namespace RecipeManagementSystem.Infrastructure.Configurations;

public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
{
    public void Configure(EntityTypeBuilder<Ingredient> builder)
    {
        builder.Property(i => i.Quantity).IsRequired()
            .HasPrecision(10, 4);;

        builder.HasOne(p => p.Product)
            .WithOne()
            .HasForeignKey<Ingredient>("ProductId")
            .IsRequired();
    }
}