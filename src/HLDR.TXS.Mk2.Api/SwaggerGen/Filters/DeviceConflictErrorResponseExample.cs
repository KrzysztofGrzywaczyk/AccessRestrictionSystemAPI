
using AccessControlSystem.SharedKernel.ApiModels;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace AccessControlSystem.Api.SwaggerGen.Filters;

public class DeviceConflictErrorResponseExample : IExamplesProvider<ServiceError>
{
    public ServiceError GetExamples()
    {
        return new ServiceError(
            "ConflictError",
            StatusCodes.Status409Conflict,
            "DeviceHasSlots",
            "Device cannot be removed because it has associated slots.",
            null,
            new List<InvalidParameter>
            {
                new("existingSlotName", "A-Slot-Name")
            });
    }
}
