
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Handlers.AccessCards;
using AccessControlSystem.UnitTests;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.UnitTests.Handlers.AccessCardsHandler.Contexts;

public class GetCardsHandlerTestsContext : UnitTestsContextBase
{
    private readonly SqlDbContext _dbContext;

    public GetCardsHandlerTestsContext()
    {
        _dbContext = SqlDbContextFactory.Create();
        Handler = new GetAccessCardsHandler(_dbContext);
    }

    public GetAccessCardsHandler Handler { get; set; }

    public async Task WithAccessCards(List<AccessCard> AccessCards)
    {
        await _dbContext.AccessCards.AddRangeAsync(AccessCards);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }
}
