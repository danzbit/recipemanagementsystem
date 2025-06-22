using Microsoft.Extensions.DependencyInjection;
using RecipeManagementSystem.Domain.Contracts;
using RecipeManagementSystem.Infrastructure.Repositories;

namespace RecipeManagementSystem.Infrastructure.Configurations;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddTransient<IRecipeRepository, RecipeRepository>();
        services.AddTransient<ICollaborationInviteRepository, CollaborationInviteRepository>();

        return services;
    }
}