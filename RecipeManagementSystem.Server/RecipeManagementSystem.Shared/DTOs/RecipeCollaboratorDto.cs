namespace RecipeManagementSystem.Shared.DTOs;

public class RecipeCollaboratorDto
{
    public string? CollaboratorEmail { get; set; }

    public DateTime JoinedAt { get; set; } = DateTime.Now;
}