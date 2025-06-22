using FluentValidation;
using RecipeManagementSystem.Shared.DTOs;

namespace RecipeManagementSystem.Application.Common.Validators;

public class RespondToInviteValidator : AbstractValidator<RespondToInviteDto>
{
    public RespondToInviteValidator()
    {
        RuleFor(x => x.InviteId)
            .NotEmpty()
            .WithMessage("Invite ID is required.");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.");
        
        RuleFor(x => x.Status)
            .NotNull()
            .IsInEnum()
            .WithMessage("Status is required.");
    }
}