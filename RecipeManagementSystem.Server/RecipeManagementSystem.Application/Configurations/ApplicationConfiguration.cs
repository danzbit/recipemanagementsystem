using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RecipeManagementSystem.Application.Caching;
using RecipeManagementSystem.Application.Commands.RecipeCommands.CreateRecipe;
using RecipeManagementSystem.Application.Common.Behaviors;
using RecipeManagementSystem.Application.Common.Mappings;
using RecipeManagementSystem.Application.Common.Validators;
using RecipeManagementSystem.Application.Contracts;
using RecipeManagementSystem.Application.Services;
using RecipeManagementSystem.Shared.Kafka;

namespace RecipeManagementSystem.Application.Configurations;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(CreateRecipeCommand).Assembly));
        services.AddValidatorsFromAssemblyContaining<RecipeDtoValidator>();
        services.AddAutoMapper(typeof(RecipeProfile).Assembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IPdfService, PdfService>();
        services.AddTransient<IKafkaProducer, KafkaProducer>();
        services.AddTransient<IResendInviteService, ResendInviteService>();
        
        services.AddSingleton<PdfStatusMemoryCache>();
        services.AddSingleton<PublishRecipeMemoryCache>();
        services.AddSingleton<ResendInviteMemoryCache>();

        return services;
    }
}