

using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Handlers.AccessCards;
using AccessControlSystem.UnitTests;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.UnitTests.Handlers.AccessCardsHandler.Contexts;

public class CreateOrUpdateCardsHandlerTestsContext : UnitTestsContextBase
{
    private SqlDbContext _dbContext;

    public CreateOrUpdateCardsHandlerTestsContext()
    {
        _dbContext = SqlDbContextFactory.Create();
        Handler = new CreateOrUpdateAccessCardsHandler(_dbContext);
    }

    public CreateOrUpdateAccessCardsHandler Handler { get; set; }

    public async Task WithAccessCards(List<AccessCard> accessCards)
    {
        await _dbContext.AccessCards.AddRangeAsync(accessCards);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task AccessCardsShouldExistInDatabase(List<string> accessCardValues)
    {
        foreach (var value in accessCardValues)
        {
            var accessCard = await _dbContext.AccessCards.FirstOrDefaultAsync(t => t.Value == value);
            accessCard.Should().NotBeNull();
        }
    }
}
