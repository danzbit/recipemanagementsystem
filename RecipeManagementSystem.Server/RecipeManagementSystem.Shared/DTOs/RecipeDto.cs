namespace RecipeManagementSystem.Shared.DTOs;

public class RecipeDto
{
    public string? Id { get; set; } = Guid.Empty.ToString();
     
    public string Title { get; init; }

    public string? Description { get; init; }

    public double Calorie { get; init; }

    public string RecipeCategory { get; init; }

    public List<IngredientDto> Ingredients { get; init; } = [];
    
    public List<RecipeCollaboratorDto> Collaborators { get; init; } = [];
    
    public string OwnerId { get; init; }
    
    public int ScheduledTime { get; set; }
}