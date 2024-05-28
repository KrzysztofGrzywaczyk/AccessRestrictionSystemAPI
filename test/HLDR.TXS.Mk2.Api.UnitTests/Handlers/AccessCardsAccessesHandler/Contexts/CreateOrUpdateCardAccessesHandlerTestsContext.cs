
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Handlers.AccessCardAccesses;
using AccessControlSystem.UnitTests;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.UnitTests.Handlers.SlotsHandler.Contexts;

public class CreateOrUpdateCardAccessesHandlerTestsContext : UnitTestsContextBase
{
    private readonly SqlDbContext _dbContext;

    public CreateOrUpdateCardAccessesHandlerTestsContext()
    {
        _dbContext = SqlDbContextFactory.Create();
        Handler = new CreateOrUpdateCardAccessesHandler(_dbContext);
    }

    public CreateOrUpdateCardAccessesHandler Handler { get; set; }

    public async Task WithDeviceAndSlotCreated(AccessControlDevice device)
    {
        await _dbContext.Devices.AddRangeAsync(device);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task WithAccessCardCreatedInDatabase(AccessCard accessCard)
    {
        await _dbContext.AccessCards.AddAsync(accessCard);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task WithAccessCardWithAccessCreated(AccessControlDevice device, AccessCard accessCard)
    {
        await _dbContext.AccessCards.AddAsync(accessCard);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        var mapping = new AccessMapping
        {
            SlotId = device.Slots!.First().SlotId,
            AccessCardId = accessCard.AccessCardId
        };

        await _dbContext.Mappings.AddAsync(mapping);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task AccessShouldBeCratedInDatabase(AccessControlDevice device, AccessCard accessCard)
    {
        var accesses = await _dbContext.Mappings.AsNoTrackingWithIdentityResolution().
            Where(m => m.SlotId == device.Slots!.First().SlotId && m.AccessCardId == accessCard.AccessCardId).ToListAsync();

        accesses.Should().NotBeNullOrEmpty();
    }
}
