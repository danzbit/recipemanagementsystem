using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeManagementSystem.Domain.Entities;

namespace RecipeManagementSystem.Infrastructure.Configurations;

public class CollaborationInviteConfiguration : IEntityTypeConfiguration<CollaborationInvite>
{
    public void Configure(EntityTypeBuilder<CollaborationInvite> builder)
    {
        builder.HasKey(i => i.Id);

        builder.HasIndex(i => new { i.RecipeId, i.InviteeId })
            .IsUnique()
            .HasFilter("[Status] = 0");

        builder.Property(i => i.Status)
            .IsRequired();

        builder.Property(i => i.SentAt)
            .IsRequired();

        builder.HasOne(i => i.Recipe)
            .WithMany(r => r.Invites)
            .HasForeignKey(i => i.RecipeId);

        builder.HasOne(i => i.Inviter)
            .WithMany(u => u.SentInvites)
            .HasForeignKey(i => i.InviterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Invitee)
            .WithMany(u => u.ReceivedInvites)
            .HasForeignKey(i => i.InviteeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}


