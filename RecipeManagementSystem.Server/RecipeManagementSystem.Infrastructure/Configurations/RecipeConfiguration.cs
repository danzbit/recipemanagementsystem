using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeManagementSystem.Domain.Entities;

namespace RecipeManagementSystem.Infrastructure.Configurations;

public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.HasIndex(u => u.Title).IsUnique();  
        builder.Property(r => r.Title).IsRequired().HasMaxLength(100);

        builder.Property(r => r.Description).HasMaxLength(200);
        
        builder.HasOne(r => r.RecipeCategory)
            .WithMany()
            .HasForeignKey("RecipeCategoryId");
        
        builder.HasMany(i => i.Ingredients)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(r => r.Owner)
            .WithMany(u => u.OwnedRecipes)
            .HasForeignKey(r => r.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(r => r.Collaborators)
            .WithOne(c => c.Recipe)
            .HasForeignKey(c => c.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(r => r.Invites)
            .WithOne(i => i.Recipe)
            .HasForeignKey(i => i.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}