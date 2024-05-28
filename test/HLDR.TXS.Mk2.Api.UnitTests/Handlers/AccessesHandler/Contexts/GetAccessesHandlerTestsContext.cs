
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Handlers.Accesses;
using AccessControlSystem.UnitTests;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.UnitTests.Handlers.AccessesHandler.Contexts;

public class GetAccessesHandlerTestsContext : UnitTestsContextBase
{
    private readonly SqlDbContext _dbContext;

    public GetAccessesHandlerTestsContext()
    {
        _dbContext = SqlDbContextFactory.Create();
        Handler = new GetAccessesHandler(_dbContext);
    }

    public GetAccessesHandler Handler { get; set; }

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

    public async Task WithAccessesCreated(AccessControlDevice device, AccessCard accessCard)
    {
        var mapping = new AccessMapping
        {
            SlotId = device.Slots!.First().SlotId,
            AccessCardId = accessCard.AccessCardId
        };

        await _dbContext.Mappings.AddAsync(mapping);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }
}
