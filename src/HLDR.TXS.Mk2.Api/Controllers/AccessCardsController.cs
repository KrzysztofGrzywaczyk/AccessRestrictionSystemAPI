using AccessControlSystem.Api.Models.DataTransferObjects;
using AccessControlSystem.Api.Models.Requests.AccessCards;
using AccessControlSystem.Api.Models.Responses.AccessCards;
using AccessControlSystem.Api.Models.SwaggerExamples;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.Controllers;

[ApiVersion("1.0")]
[Route("api/v{api-version:apiVersion}/accesscards/")]
[ApiController]
public class AccessCardsController(IMediator mediator) : BaseController(mediator)
{
    [HttpGet]
    [SwaggerOperation(Tags = new[] { "AccessCards" }, Summary = "Retrieves all AccessCards.", Description = "Retrieves a list of all AccessCards defined in the system.")]
    [Produces("application/json")]
    [SwaggerResponse(200, "OK: AccessCards retrieved.", typeof(DataResponseModel<AccessCardValueList>))]
    [SwaggerResponse(404, "NotFound: AccessCards not found.")]
    public async Task<IActionResult> GetAccessCards([FromQuery] GetAccessCardsRequest request, CancellationToken cancellationToken = default)
        => await Send<GetAccessCardsRequest, GetAccessCardsResponse>(request, cancellationToken);

    [HttpPut]
    [SwaggerOperation(Tags = new[] { "AccessCards" }, Summary = "Creates or updates AccessCards", Description = "Creates or updates AccessCards in the AccessControlSystem Device.")]
    [Produces("application/json")]
    [SwaggerResponse(204, "No Content: AccessCards successfully added or already exist.")]
    [SwaggerResponse(400, "Bad Request: Invalid AccessCards request.")]
    public async Task<IActionResult> CreateOrUpdateAccessCards([FromBody] PutAccessCardsRequest request,  CancellationToken cancellationToken = default)
        => await Send<PutAccessCardsRequest, PutAccessCardsResponse>(request, cancellationToken);

    [HttpDelete]
    [SwaggerOperation(Tags = new[] { "AccessCards" }, Summary = "Removes AccessCards", Description = "Removes all or specified AccessCards")]
    [Produces("application/json")]
    [SwaggerResponse(204, "No Content: AccessCards removed.")]
    [SwaggerResponse(207, "Multi-Status: Some AccessCards could not be removed because they have associated accesses.",  typeof(DataResponseModel<RemovedAndWithAccessesAccessCards>))]
    [SwaggerResponse(400, "Bad Request: Invalid AccessCards request.")]
    [SwaggerResponse(404, "Not Found: AccessCards relating to the given values not found.")]
    [SwaggerResponse(409, "Conflict: AccessCards can not be removed because they all have associated accesses.")]

    public async Task<IActionResult> DeleteAccessCards([FromQuery] RemoveAccessCardsRequest request, CancellationToken cancellationToken = default)
        => await Send<RemoveAccessCardsRequest, RemoveAccessCardsResponse>(request, cancellationToken);
}
