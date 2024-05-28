
using AccessControlSystem.Api.Models.Responses.AccessCards;
using MediatR;
using System.Collections.Generic;

namespace AccessControlSystem.Api.Models.Requests.AccessCards;

public class PutAccessCardsRequest : IRequest<PutAccessCardsResponse>
{
    public List<string>? AccessCardValues { get; set; }
}
