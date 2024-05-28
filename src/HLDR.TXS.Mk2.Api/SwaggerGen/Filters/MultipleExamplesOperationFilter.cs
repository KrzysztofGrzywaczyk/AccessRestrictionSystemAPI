
using AccessControlSystem.Api.Controllers;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace AccessControlSystem.Api.SwaggerGen.Filters;

public class MultipleExamplesOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Check if the operation is the one you want to add multiple examples for
        if (context.MethodInfo.DeclaringType == typeof(CardsAccessesController) && context.MethodInfo.Name == "CreateOrUpdateAccessCardAccesses")
        {
            operation.RequestBody.Content["application/json"].Examples = new Dictionary<string, OpenApiExample>
            {
                {
                    "Slot Access By slotName", new OpenApiExample
                    {
                        Summary = "Slot Access By slotName",
                        Value = new OpenApiObject
                        {
                            ["slotAccesses"] = new OpenApiArray
                            {
                                new OpenApiObject
                                {
                                    ["deviceId"] = new OpenApiString("860858060945938"),
                                    ["slotName"] = new OpenApiString("Onsite")
                                },
                                new OpenApiObject
                                {
                                    ["deviceId"] = new OpenApiString("861641049362055"),
                                    ["slotName"] = new OpenApiString("Offsite")
                                }
                            }
                        }
                    }
                },
                {
                    "Slot Access By slotNumber", new OpenApiExample
                    {
                        Summary = "Slot Access By slotNumber",
                        Value = new OpenApiObject
                        {
                            ["slotAccesses"] = new OpenApiArray
                            {
                                new OpenApiObject
                                {
                                    ["deviceId"] = new OpenApiString("860858060945938"),
                                    ["slotNumber"] = new OpenApiInteger(1)
                                },
                                new OpenApiObject
                                {
                                    ["deviceId"] = new OpenApiString("860858060945938"),
                                    ["slotNumber"] = new OpenApiInteger(3)
                                },
                                new OpenApiObject
                                {
                                    ["deviceId"] = new OpenApiString("861641049362055"),
                                    ["slotNumber"] = new OpenApiInteger(2)
                                },
                                new OpenApiObject
                                {
                                    ["deviceId"] = new OpenApiString("861641049362055"),
                                    ["slotNumber"] = new OpenApiInteger(4)
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
