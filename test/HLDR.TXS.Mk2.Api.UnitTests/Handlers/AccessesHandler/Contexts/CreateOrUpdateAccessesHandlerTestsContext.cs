
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Handlers.Accesses;
using AccessControlSystem.UnitTests;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.UnitTests.Handlers.AccessesHandler.Contexts;

public class CreateOrUpdateAccessesHandlerTestsContext : UnitTestsContextBase
{
    private readonly SqlDbContext _dbContext;

    public CreateOrUpdateAccessesHandlerTestsContext()
    {
        _dbContext = SqlDbContextFactory.Create();
        Handler = new CreateOrUpdateAccessesHandler(_dbContext);
    }

    public CreateOrUpdateAccessesHandler Handler { get; set; }

    public async Task WithDeviceAndSlotCreated(AccessControlDevice device)
    {
        await _dbContext.Devices.AddRangeAsync(device);
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
    }

    public async Task AccessShouldBeCreatedInDatabase(AccessControlDevice device, AccessCard accessCard)
    {
        var access = await _dbContext.Mappings
            .FirstOrDefaultAsync(m => m.SlotId == device.Slots!.First().SlotId && m.AccessCardId == accessCard.AccessCardId);

        access.Should().NotBeNull();
        access.Should().BeOfType<AccessMapping>();
    }
}
