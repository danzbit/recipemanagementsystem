using FluentValidation;
using RecipeManagementSystem.Shared.DTOs;

namespace RecipeManagementSystem.Application.Common.Validators;

public class SendInviteValidator : AbstractValidator<SendInviteDto>
{
    public SendInviteValidator()
    {
        RuleFor(x => x.RecipeId)
            .NotEmpty()
            .WithMessage("Recipe ID is required.");

        RuleFor(x => x.InviteeId)
            .NotEmpty()
            .WithMessage("Invitee ID is required.");

        RuleFor(x => x.InviterId)
            .NotEmpty()
            .WithMessage("Inviter ID is required.");
    }
}