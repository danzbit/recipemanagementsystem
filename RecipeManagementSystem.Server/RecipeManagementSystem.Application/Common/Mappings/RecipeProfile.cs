using AutoMapper;
using RecipeManagementSystem.Domain.Entities;
using RecipeManagementSystem.Shared.DTOs;

namespace RecipeManagementSystem.Application.Common.Mappings;

public class RecipeProfile : Profile
{
    public RecipeProfile()
    {
        CreateMap<RecipeDto, Recipe>()
            .ForMember(dest => dest.RecipeCategory,
                opt => opt.MapFrom(src => new RecipeCategory { Name = src.RecipeCategory }))
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients));
        
        CreateMap<Recipe, RecipeDto>()
            .ForMember(dest => dest.RecipeCategory, opt => opt.MapFrom(src => src.RecipeCategory.Name))
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients))
            .ForMember(dest => dest.Collaborators, opt => opt.MapFrom(src => src.Collaborators))
            .AfterMap((src, dest) =>
            {
                for (int i = 0; i < src.Collaborators.Count && i < dest.Collaborators.Count; i++)
                {
                    dest.Collaborators[i].CollaboratorEmail = src.Collaborators[i].User.Email;
                }
            });
    }
}