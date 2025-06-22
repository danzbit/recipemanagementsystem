using RecipeManagementSystem.Shared.Pdf;

namespace RecipeManagementSystem.Import.Contracts;

public interface IPdfStatusUpdater
{
    Task UpdateStatusAsync(PdfStatus update);
}