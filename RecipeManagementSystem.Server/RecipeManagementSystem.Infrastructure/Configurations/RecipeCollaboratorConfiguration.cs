using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeManagementSystem.Domain.Entities;

namespace RecipeManagementSystem.Infrastructure.Configurations;

public class RecipeCollaboratorConfiguration : IEntityTypeConfiguration<RecipeCollaborator>
{
    public void Configure(EntityTypeBuilder<RecipeCollaborator> builder)
    {
        builder.HasKey(rc => rc.Id);

        builder.HasIndex(rc => new { rc.RecipeId, rc.UserId }).IsUnique();

        builder.HasOne(rc => rc.Recipe)
            .WithMany(r => r.Collaborators)
            .HasForeignKey(rc => rc.RecipeId);

        builder.HasOne(rc => rc.User)
            .WithMany(u => u.Collaborations)
            .HasForeignKey(rc => rc.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
