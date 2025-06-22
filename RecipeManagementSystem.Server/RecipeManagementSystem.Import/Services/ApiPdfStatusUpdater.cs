using System.Net.Http.Json;
using RecipeManagementSystem.Import.Contracts;
using RecipeManagementSystem.Shared.Pdf;

namespace RecipeManagementSystem.Import.Services;

public class ApiPdfStatusUpdater(
    IHttpClientFactory httpClientFactory)
    : IPdfStatusUpdater
{
    public async Task UpdateStatusAsync(PdfStatus update)
    {
        var httpClient = httpClientFactory.CreateClient(nameof(ApiPdfStatusUpdater));

        var response = await httpClient.PostAsJsonAsync($"api/pdf/status/", update);
        response.EnsureSuccessStatusCode();
    }
}