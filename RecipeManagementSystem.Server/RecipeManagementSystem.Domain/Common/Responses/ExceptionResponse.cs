using System.Net;

namespace RecipeManagementSystem.Domain.Common.Responses;

public record ExceptionResponse(HttpStatusCode StatusCode, string Description);