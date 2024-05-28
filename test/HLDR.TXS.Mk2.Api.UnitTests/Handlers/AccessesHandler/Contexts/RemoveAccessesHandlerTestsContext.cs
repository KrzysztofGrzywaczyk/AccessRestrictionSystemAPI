
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Handlers.Accesses;
using AccessControlSystem.UnitTests;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.UnitTests.Handlers.AccessesHandler.Contexts;

public class RemoveAccessesHandlerTestsContext : UnitTestsContextBase
{
    private readonly SqlDbContext _dbContext;

    public RemoveAccessesHandlerTestsContext()
    {
        _dbContext = SqlDbContextFactory.Create();
        Handler = new RemoveAccessesHandler(_dbContext);
    }

    public RemoveAccessesHandler Handler { get; set; }

    public async Task WithDeviceAndSlotCreated(AccessControlDevice device)
    {
        await _dbContext.Devices.AddAsync(device);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task WithAccessCardCreated(AccessCard accessCard)
    {
        await _dbContext.AccessCards.AddAsync(accessCard);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task WithAccessCreated(AccessControlDevice device, AccessCard accessCard)
    {
        var mapping = new AccessMapping
        {
            SlotId = device.Slots!.First().SlotId,
            AccessCardId = accessCard.AccessCardId
        };

        await _dbContext.Mappings.AddAsync(mapping);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        _dbContext.Entry(mapping).State = EntityState.Detached;
    }

    public async Task AccessesShouldNotExistInDatabase(AccessControlDevice device, AccessCard accessCard)
    {
        var accesses = await _dbContext.Mappings.AsNoTrackingWithIdentityResolution().
            Where(m => m.SlotId == device.Slots!.First().SlotId && m.AccessCardId == accessCard.AccessCardId).ToListAsync();
        accesses.Should().BeNullOrEmpty();
    }

    public async Task AccessToLeaveShouldStillExistsInDatabase(AccessControlDevice device, AccessCard accessCardToLeave)
    {
        var accesses = await _dbContext.Mappings.AsNoTrackingWithIdentityResolution().
            Where(m => m.SlotId == device.Slots!.First().SlotId && m.AccessCardId == accessCardToLeave.AccessCardId).ToListAsync();

        accesses.Should().NotBeNullOrEmpty();
    }
}
