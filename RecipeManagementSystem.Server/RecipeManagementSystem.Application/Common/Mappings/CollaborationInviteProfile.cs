using AutoMapper;
using RecipeManagementSystem.Domain.Entities;
using RecipeManagementSystem.Shared.DTOs;

namespace RecipeManagementSystem.Application.Common.Mappings;

public class CollaborationInviteProfile : Profile
{
    public CollaborationInviteProfile()
    {
        CreateMap<SendInviteDto, CollaborationInvite>();

        CreateMap<CollaborationInvite, CollaborationInviteDto>();
    }
}