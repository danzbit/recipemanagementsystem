namespace RecipeManagementSystem.Shared.Errors;

public static class PdfErrors
{
    public static readonly Error PdfStatusNotFound = new(ErrorCode.PdfStatusNotFound, "PDF status not found.");
    
    public static readonly Error PdfIsNotCompletedYet = new(ErrorCode.PdfIsNotCompletedYet, "PDF is not ready yet.");
    
    public static readonly Error PdfFileNotExist = new(ErrorCode.PdfFileNotExist, "PDF file does not exist on disk.");
}