using RecipeManagementSystem.Domain.Enums;
using RecipeManagementSystem.Shared.DTOs;

namespace RecipeManagementSystem.Application.Extensions;

public static class InviteStatusExtensions
{
    public static InviteStatus ConvertFromDto(this InviteStatusDto inviteStatusDto)
    {
        return (InviteStatus)(int)inviteStatusDto;
    }
}