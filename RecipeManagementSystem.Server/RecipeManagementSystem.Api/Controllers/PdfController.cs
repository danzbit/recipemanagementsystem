using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeManagementSystem.Application.Contracts;
using RecipeManagementSystem.Domain.Models;
using RecipeManagementSystem.Shared.Models;
using RecipeManagementSystem.Shared.Pdf;

namespace RecipeManagementSystem.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class PdfController(IPdfService pdfService) : ControllerBase
{
    [HttpPost("generatePdf")]
    public async Task<IActionResult> GeneratePdf([FromQuery] string recipeId)
    {
        var result = await pdfService.GeneratePdf(recipeId);
        return result.ToActionResult();
    }

    [HttpPost("status")]
    [AllowAnonymous]
    public async Task<IActionResult> UpdateStatus([FromBody] PdfStatus pdfStatus)
    {
        var result = await pdfService.UpdateStatus(pdfStatus);
        return result.ToActionResult();
    }

    [HttpGet("download/{requestId}")]
    public async Task<IActionResult> DownloadPdf([FromRoute] string requestId)
    {
        var result = await pdfService.GetFileBytes(requestId);
        return result.ToActionResultForExportFile(result.Value, $"recipe.pdf");
    }
}