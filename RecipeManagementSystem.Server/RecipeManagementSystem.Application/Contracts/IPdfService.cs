using RecipeManagementSystem.Shared.Models;
using RecipeManagementSystem.Shared.Pdf;

namespace RecipeManagementSystem.Application.Contracts;

public interface IPdfService
{
    Task<Result<Guid>> GeneratePdf(string recipeId);
    
    Task<Result> UpdateStatus(PdfStatus status);

    Task<Result<byte[]>> GetFileBytes(string requestId);
}