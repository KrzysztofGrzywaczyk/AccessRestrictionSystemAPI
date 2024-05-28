
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace AccessControlSystem.SharedKernel.Logging;

[ExcludeFromCodeCoverage]
public static class LoggingExtensions
{
    public static IServiceCollection AddLogger(this IServiceCollection services) => services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

    public static IServiceCollection AddLogger(this IServiceCollection services, string categoryName) =>
        services.AddSingleton(serviceProvider => new Logger(serviceProvider.GetRequiredService<ILoggerFactory>(), categoryName));
}