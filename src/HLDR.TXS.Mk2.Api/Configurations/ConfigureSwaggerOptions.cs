
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace AccessControlSystem.Api.Configurations;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private const string ApiTitle = "AccessControlSystem API";

    private const string ApiDescription = "This AccessControlSystem API is a complete access control system that enables access restrictions management using devices that control access via access cards. It consists of an arbitrary number of devices. Each device has 8 slots (doors, gates, access stations, etc.) which can be associated with specific access cards by assigning access permissions.";

    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

    public void Configure(SwaggerGenOptions options)
    {
        options.DescribeAllParametersInCamelCase();

        options.AddSecurityDefinition("api-key", new OpenApiSecurityScheme
        {
            Description = "ApiKey is mandatory in query",
            Type = SecuritySchemeType.ApiKey,
            Name = "api-key",
            In = ParameterLocation.Query,
            Scheme = "ApiKeyScheme"
        });

        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
                description.GroupName,
                new OpenApiInfo
                {
                    Title = $"{ApiTitle} {description.ApiVersion}",
                    Description = ApiDescription,
                    Version = description.ApiVersion.ToString(),
                });
        }

        var key = new OpenApiSecurityScheme()
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "api-key"
            },
            In = ParameterLocation.Query
        };
        var requirement = new OpenApiSecurityRequirement
        {
            { key, new List<string>() }
        };

        options.AddSecurityRequirement(requirement);
    }
}
