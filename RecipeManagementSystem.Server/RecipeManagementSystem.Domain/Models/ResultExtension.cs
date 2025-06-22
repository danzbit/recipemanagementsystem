using Microsoft.AspNetCore.Mvc;
using RecipeManagementSystem.Domain.Common.Responses;
using RecipeManagementSystem.Domain.Entities;
using RecipeManagementSystem.Shared.Errors;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Domain.Models;

public static class ResultExtension
{
    public static IActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
        {
            return new OkResult();
        }

        return CreateErrorResponse(result.Error);
    }
    
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return new OkObjectResult(result.Value);
        }
        
        if (result.IsFailure && result.Value is List<string>)
        {
            return new BadRequestObjectResult(new { Errors = result.Value });
        }

        return CreateErrorResponse(result.Error);
    }


    public static IActionResult ToActionResultForPagedList(this Result<PagedList<Recipe>> result)
    {
        if (result.IsSuccess)
        {
            var entities = new PagedRecipeListResponse(result.Value, result.Value.TotalPages);
            return new OkObjectResult(entities);
        }

        return CreateErrorResponse(result.Error);
    }
    
    public static IActionResult ToActionResultForExportFile(this Result result, byte[] bytes, string fileName)
    {
        if (result.IsSuccess)
        {
            return new FileContentResult(bytes, "application/json")
            {
                FileDownloadName = fileName
            };
        }

        return CreateErrorResponse(result.Error);
    }

    private static IActionResult CreateErrorResponse(Error error)
    {
        var message = new { message = error.Description };
        return error.Code switch
        {
            ErrorCode.None => new BadRequestObjectResult(message),
            ErrorCode.EntityNotFound => new NotFoundObjectResult(message),
            ErrorCode.InvalidId => new BadRequestObjectResult(message),
            ErrorCode.UserNotFound => new NotFoundObjectResult(message),
            ErrorCode.Unauthorized => new BadRequestObjectResult(message),
            ErrorCode.UserAlreadyExist => new BadRequestObjectResult(message),
            ErrorCode.UpdateFailed => new BadRequestObjectResult(message),
            ErrorCode.AddFailed => new BadRequestObjectResult(message),
            ErrorCode.UserCreation => new BadRequestObjectResult(message),
            ErrorCode.PdfFileNotExist => new NotFoundObjectResult(message),
            ErrorCode.PdfIsNotCompletedYet => new BadRequestObjectResult(message),
            ErrorCode.PdfStatusNotFound => new NotFoundObjectResult(message),
            ErrorCode.CollaborationInviteAlreadySent => new BadRequestObjectResult(message),
            ErrorCode.CannotAcceptInvite =>  new BadRequestObjectResult(message),
            _ => throw new NotSupportedException()
        };
    }
}