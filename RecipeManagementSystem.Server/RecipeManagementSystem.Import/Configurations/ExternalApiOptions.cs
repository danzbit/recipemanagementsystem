namespace RecipeManagementSystem.Import.Configurations;

public class ExternalApiOptions
{
    public const string ExternalApi = "ExternalApi";
    
    public string BaseUrl { get; set; } = string.Empty;

    public int TimeoutSeconds { get; set; } = 30;
}