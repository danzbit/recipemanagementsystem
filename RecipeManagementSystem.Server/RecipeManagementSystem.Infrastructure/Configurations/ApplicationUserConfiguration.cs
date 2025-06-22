using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeManagementSystem.Domain.Entities;

namespace RecipeManagementSystem.Infrastructure.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasMany(u => u.OwnedRecipes)
            .WithOne(r => r.Owner)
            .HasForeignKey(r => r.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.Collaborations)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.SentInvites)
            .WithOne(i => i.Inviter)
            .HasForeignKey(i => i.InviterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.ReceivedInvites)
            .WithOne(i => i.Invitee)
            .HasForeignKey(i => i.InviteeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}