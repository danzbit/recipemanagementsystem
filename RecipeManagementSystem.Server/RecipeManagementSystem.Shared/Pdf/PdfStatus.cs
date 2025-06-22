namespace RecipeManagementSystem.Shared.Pdf;

public class PdfStatus
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    public PdfStatusType Status { get; set; } =  PdfStatusType.Queued;

    public Guid RecipeId { get; set; }
}