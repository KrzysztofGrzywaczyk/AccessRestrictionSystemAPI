
using AccessControlSystem.Api.Models.Bindings;
using AccessControlSystem.Api.Models.Responses.AccessCards;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace AccessControlSystem.Api.Models.Requests.AccessCards;

public class RemoveAccessCardsRequest : IRequest<RemoveAccessCardsResponse>
{
    [SwaggerParameter(Description = "An optional comma-separated list of AccessCard values.")]
    [CommaSeparated]
    public string[]? AccessCardValues { get; set; }
}
