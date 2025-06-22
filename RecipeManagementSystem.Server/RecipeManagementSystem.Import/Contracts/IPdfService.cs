using RecipeManagementSystem.Shared.Models;
using RecipeManagementSystem.Shared.Pdf;

namespace RecipeManagementSystem.Import.Contracts;

public interface IPdfService
{
    Task GenerateAsync(PdfRequest request);
}