
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Handlers.AccessCards;
using AccessControlSystem.UnitTests;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.UnitTests.Handlers.AccessCardsHandler.Contexts;

public class RemoveCardsHandlerTestsContext : UnitTestsContextBase
{
    private readonly SqlDbContext _dbContext;

    public RemoveCardsHandlerTestsContext()
    {
        _dbContext = SqlDbContextFactory.Create();
        Handler = new RemoveAccessCardsHandler(_dbContext);
    }

    public RemoveAccessCardsHandler Handler { get; set; }

    public async Task WithAccessCards(List<AccessCard> AccessCards)
    {
        await _dbContext.AccessCards.AddRangeAsync(AccessCards);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task AccessCardsShouldNotExistsInDatabase(List<string> AccessCardValues)
    {
        foreach (var value in AccessCardValues)
        {
            var AccessCard = await _dbContext.AccessCards.FirstOrDefaultAsync(t => t.Value == value);
            AccessCard.Should().BeNull();
        }
    }
}
