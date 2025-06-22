using System.Text.Json;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using RecipeManagementSystem.Import.Contracts;
using RecipeManagementSystem.Shared.Pdf;

namespace RecipeManagementSystem.Import.Services;

public class PdfService : IPdfService
{
    private readonly string _output = Path.Combine(Directory.GetCurrentDirectory(), "output");
    private readonly IPdfStatusUpdater _updater;
    private readonly ILogger<PdfService> _logger;

    public PdfService(ILogger<PdfService> logger, IPdfStatusUpdater updater)
    {
        _logger = logger;
        _updater = updater;
        Directory.CreateDirectory(_output);
    }

    public async Task GenerateAsync(PdfRequest request)
    {
        var status = new PdfStatus
        {
            Id = request.Id.ToString(),
            Status = PdfStatusType.Processing,
            RecipeId = request.RecipeId,
        };

        try
        {
            try
            {
                await CallSaveStatus(status);
            }
            catch (Exception ex)
            {
                _logger.LogError("[ERROR] Failed to update status to 'Processing': {ExMessage}", ex.Message);
                throw;
            }

            var recipe = request.Recipe!;
            var jsonString = JsonSerializer.Serialize(recipe, new JsonSerializerOptions { WriteIndented = true });

            var safeTitle = string.Concat(recipe.Title.Where(c => !Path.GetInvalidFileNameChars().Contains(c)));
            var filePath = Path.Combine(_output, $"{safeTitle}.json");
            
            if (!Directory.Exists(_output))
            {
                Directory.CreateDirectory(_output);
            }
            
            await File.WriteAllTextAsync(filePath, jsonString);
            
            try
            {
                await CallSaveStatus(status, PdfStatusType.Completed);
            }
            catch (Exception ex)
            {
                _logger.LogError("[ERROR] Failed to update status to 'Processing': {ExMessage}", ex.Message);
                throw;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("[FAILURE] JSON generation failed: {ExMessage}", ex.Message);

            try
            {
                await CallSaveStatus(status, PdfStatusType.Failed);
            }
            catch (Exception updateEx)
            {
                _logger.LogError("[ERROR] Failed to update status to 'Failed': {UpdateExMessage}", updateEx.Message);
            }
        }
    }

    private async Task CallSaveStatus(PdfStatus status, PdfStatusType statusType = PdfStatusType.None)
    {
        if (statusType != PdfStatusType.None)
        {
            status.Status = statusType;
        }

        await _updater.UpdateStatusAsync(status);
    }
}