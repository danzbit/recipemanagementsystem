using RecipeManagementSystem.Shared.Pdf;

namespace RecipeManagementSystem.Application.Contracts;

public interface IHubClient
{
    Task RecipeCreated(string message);
    
    Task PdfStatusUpdated(PdfStatus status);
    
    Task ResendInvite(string message);
    
    Task AutoExpireInvite(string message);
}