using AutoMapper;
using RecipeManagementSystem.Domain.Entities;
using RecipeManagementSystem.Shared.DTOs;

namespace RecipeManagementSystem.Application.Common.Mappings;

public class RecipeCollaboratorProfile : Profile
{
    public RecipeCollaboratorProfile()
    {
        CreateMap<RespondToInviteDto, RecipeCollaborator>();

        CreateMap<RecipeCollaborator, RecipeCollaboratorDto>();
        
        CreateMap<CollaborationInvite, RecipeCollaborator>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.InviteeId));
    }
}