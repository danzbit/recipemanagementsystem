using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeManagementSystem.Domain.Entities;

namespace RecipeManagementSystem.Infrastructure.Configurations;

public class RecipeCategoryConfiguration : IEntityTypeConfiguration<RecipeCategory>
{
    public void Configure(EntityTypeBuilder<RecipeCategory> builder)
    {
        builder.HasKey(p => p.Name);
        
        builder.HasData(
            new RecipeCategory { Name = "Breakfast" },
            new RecipeCategory { Name = "Lunch" },
            new RecipeCategory { Name = "Dinner" },
            new RecipeCategory { Name = "Snacks" },
            new RecipeCategory { Name = "Desserts" }
        );
    }
}