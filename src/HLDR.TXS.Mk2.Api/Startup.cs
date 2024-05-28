
using AccessControlSystem.Api.Configurations;
using AccessControlSystem.Api.Const;
using AccessControlSystem.Api.Extensions;
using AccessControlSystem.Api.Middlewares;
using AccessControlSystem.Api.Models;
using AccessControlSystem.Api.SwaggerGen.Filters;
using AccessControlSystem.Api.Validation;
using AccessControlSystem.SharedKernel.ApplicationInsights;
using AccessControlSystem.SharedKernel.Behaviors;
using FluentValidation;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AccessControlSystem.Api;

[ExcludeFromCodeCoverage]
public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(WebApplication app)
    {
        app.UseMiddleware<ErrorHandlingMiddleware>();
        app.MapControllers();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var descriptions = app.DescribeApiVersions();
            foreach (var description in descriptions.Reverse())
            {
                var url = $"/swagger/{description.GroupName}/swagger.json";
                var name = description.GroupName.ToUpperInvariant();
                options.SwaggerEndpoint(url, name);
            }
        });

        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.UseHealthChecks("/health/no-dependencies", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            Predicate = hc => !hc.Tags.Contains(HealthCheckKeys.DependencyTag)
        });

        app.UseMiddleware<ApiKeyValidationMiddleware>();
        ValidatorOptions.Global.PropertyNameResolver = (_, memberInfo, expression) => CamelCasePropertyNameResolver.ResolvePropertyName(memberInfo, expression);
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddAppInsightsTelemetry(_configuration);

        services.AddHttpClient();

        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.OperationFilter<QueryParamStyleOperationFilter>();
            c.OperationFilter<MultipleExamplesOperationFilter>();
            c.ExampleFilters();
        });

        services.AddSwaggerExamplesFromAssemblyOf<DeviceConflictErrorResponseExample>();

        services
            .AddVersioning()
            .AddProjectHealthChecks()
            .AddProjectMediator<Startup>()
            .AddProjectLogging()
            .AddServices()
            .ConfigureWithStatic<AzureSqlConfiguration>()
            .ConfigureWithStatic<AuthorizationConfiguration>()
            .AddProjectDatabase(_configuration);
    }
}
