using FluentValidation;
using RecipeManagementSystem.Shared.DTOs;

namespace RecipeManagementSystem.Application.Common.Validators;

public class RecipeDtoValidator : AbstractValidator<RecipeDto>
{
    private static readonly string[] AllowedCategories = ["Breakfast", "Lunch", "Dinner", "Snack", "Desserts"];

    public RecipeDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Title is required.");

        RuleFor(x => x.Description)
            .MaximumLength(200);

        RuleFor(x => x.RecipeCategory)
            .NotEmpty().WithMessage("Recipe category is required.");

        RuleFor(x => x.RecipeCategory)
            .Must(category => AllowedCategories.Contains(category))
            .WithMessage("Invalid recipe category.");

        RuleFor(x => x.OwnerId)
            .NotEmpty()
            .WithMessage("Owner ID is required.");

        RuleForEach(x => x.Ingredients)
            .SetValidator(new IngredientDtoValidator());
    }
}