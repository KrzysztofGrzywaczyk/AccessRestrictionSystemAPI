
using AccessControlSystem.Api.Configurations;
using AccessControlSystem.Api.Entities;
using AccessControlSystem.SharedKernel.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics.CodeAnalysis;

namespace AccessControlSystem.Api.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProjectLogging(this IServiceCollection services) => services.AddLogger();

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddHttpClient();
        return services;
    }

    public static IServiceCollection AddProjectDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var azureSqlConfiguration = configuration.Get<AzureSqlConfiguration>();
        services.AddDbContext<SqlDbContext>(options => options.UseSqlServer(azureSqlConfiguration?.SqlConnectionString));

        return services;
    }

    public static IServiceCollection AddProjectHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks();

        return services;
    }

    public static IServiceCollection AddVersioning(this IServiceCollection services)
    {
        services
            .AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVVV";

                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
                options.SubstitutionFormat = "VVVV";
            });

        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen();

        return services;
    }

    public static IServiceCollection ConfigureWithStatic<TConfiguration>(this IServiceCollection self, string configSectionPath = "")
        where TConfiguration : class
    {
        self.AddOptions<TConfiguration>()
           .BindConfiguration(configSectionPath)
           .ValidateDataAnnotations()
           .ValidateOnStart();
        self.AddSingleton(p => p.GetRequiredService<IOptions<TConfiguration>>().Value);
        return self;
    }
}