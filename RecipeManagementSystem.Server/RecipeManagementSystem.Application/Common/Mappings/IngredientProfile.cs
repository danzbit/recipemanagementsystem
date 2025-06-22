using AutoMapper;
using RecipeManagementSystem.Domain.Entities;
using RecipeManagementSystem.Shared.DTOs;

namespace RecipeManagementSystem.Application.Common.Mappings;

public class IngredientProfile : Profile
{
    public IngredientProfile()
    {
        CreateMap<IngredientDto, Ingredient>()
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => new Product { Id = src.ProductId ?? Guid.NewGuid(), Name = src.Product }));

        CreateMap<Ingredient, IngredientDto>()
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.Id));
    }
}