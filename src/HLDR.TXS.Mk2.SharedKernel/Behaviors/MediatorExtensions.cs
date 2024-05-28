
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AccessControlSystem.SharedKernel.Behaviors;

public static class MediatorExtensions
{
    public static IServiceCollection AddProjectMediator<TAssembly>(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining(typeof(TAssembly)));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.Scan(reg => reg
            .FromAssemblies(typeof(TAssembly).Assembly)
            .AddClasses(c => c
                .AssignableTo(typeof(IValidator<>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        return services;
    }
}