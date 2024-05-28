
using AccessControlSystem.Api.Models.DataTransferObjects;
using AccessControlSystem.Api.Models.Requests.AccessCardAccesses;
using AccessControlSystem.Api.Models.Responses.AccessCardAccesses;
using AccessControlSystem.Api.Models.SwaggerExamples;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.Controllers;

[ApiVersion("1.0")]
[Route("api/v{api-version:apiVersion}/accesscards/{accessCardValue}/accesses")]
[ApiController]
public class CardsAccessesController(IMediator mediator) : BaseController(mediator)
{
    [HttpGet]
    [SwaggerOperation(Tags = new[] { "Access Card Accesses" }, Summary = "Retrieves all or filtered accesses of an AccessCard", Description = "Retrieves all accesses associated with an AccessCard or accesses scoped via device Id, and further, by providing list of slots of device expressed either by slot names OR slot numbers (device Id is mandatory to be specified if slots are specified).")]
    [Produces("application/json")]
    [SwaggerResponse(200, "OK: AccessCard accesses retrieved.", typeof(DataResponseModel<AccessCardAccessesDto>))]
    [SwaggerResponse(400, "Bad Request: \n - A list of slots expressed as slot names AND slot numbers; only one OR other is allowed. \n - List of slots provided without deviceId. If slots are specified, device Id is mandatory.")]
    [SwaggerResponse(404, "Not Found: \n - AccessCard not found. \n - Accesses not found.")]
    public async Task<IActionResult> GetAccessCardAccesses([FromQuery] GetCardAccessesRequest request, CancellationToken cancellationToken = default)
        => await Send<GetCardAccessesRequest, GetCardAccessesResponse>(request, cancellationToken);

    [HttpPut]
    [SwaggerOperation(Tags = new[] { "Access Card Accesses" }, Summary = "Assigns slot accesses to an AccessCard", Description = "Assigns slot accesses to an AccessCard by providing a list of slots in the form.")]
    [Produces("application/json")]
    [SwaggerResponse(204, "No Content: Slot accesses were assigned to the AccessCard.")]
    [SwaggerResponse(207, "Multi-Status: Some slot accesses could not be assigned.", typeof(DataResponseModel<AssignedAndFailedCardAccesses>))]
    [SwaggerResponse(400, "Bad Request: Invalid card accesses request.")]
    public async Task<IActionResult> CreateOrUpdateAccessCardAccesses([FromRoute][SwaggerParameter("An access card value that is presented to the device.")] string accessCardValue, [FromBody] PutCardAccessesRequest request, CancellationToken cancellationToken = default)
    {
        request.AccessCardValue = accessCardValue;
        return await Send<PutCardAccessesRequest, PutCardAccessesResponse>(request, cancellationToken);
    }

    [HttpDelete]
    [SwaggerOperation(Tags = new[] { "Access Card Accesses" }, Summary = "Removes slot accesses from an AccessCard", Description = "Removes all accesses associated with an AccessCard or scoped. This endpoint support multiple scenarios: 1. Removes all access from an AccessCard across all devices. 2. Removes all access from an AccessCard on specified devices (passed: deviceId). 3. Removes all access from an AccessCard on specified Slots (passed : deviceId + slot names OR slot numbers). Device Id is mandatory when slots are passed.")]
    [Produces("application/json")]
    [SwaggerResponse(204, "No Content: The specified access was removed from the AccessCard.")]
    [SwaggerResponse(400, "Bad Request: \n - A list of Slots expressed as slot names AND slot numbers; only one OR the other is allowed. \n - List of slots provided without deviceId. Device Id is mandatory in this case.")]
    [SwaggerResponse(404, "Not Found: \n - AccessCard not found. \n - Accesses not found.")]
    public async Task<IActionResult> DeleteAccessCardAccesses([FromQuery] RemoveCardAccessesRequest request, CancellationToken cancellationToken = default)
        => await Send<RemoveCardAccessesRequest, RemoveCardAccessesResponse>(request, cancellationToken);
}
