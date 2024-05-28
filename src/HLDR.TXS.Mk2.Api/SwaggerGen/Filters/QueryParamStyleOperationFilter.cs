
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace AccessControlSystem.Api.SwaggerGen.Filters;

public class QueryParamStyleOperationFilter() : IOperationFilter
{
    private readonly Dictionary<string, (ParameterStyle Style, bool Explode)> _parametersConfig = new()
        {
            ["deviceIds"] = (ParameterStyle.Form, false),
            ["slotNames"] = (ParameterStyle.Form, false),
            ["slotNumbers"] = (ParameterStyle.Form, false),
            ["accessCardValues"] = (ParameterStyle.Form, false),
        };

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        foreach (var paramConfig in _parametersConfig)
        {
            var parameter = operation.Parameters.FirstOrDefault(p => p.Name == paramConfig.Key && p.In == ParameterLocation.Query);
            if (parameter != null)
            {
                parameter.Style = paramConfig.Value.Style;
                parameter.Explode = paramConfig.Value.Explode;
            }
        }
    }
}
