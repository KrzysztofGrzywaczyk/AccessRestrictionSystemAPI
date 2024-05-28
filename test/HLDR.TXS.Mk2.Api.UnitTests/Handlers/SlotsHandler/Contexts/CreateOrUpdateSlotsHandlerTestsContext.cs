
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Handlers.Slots;
using AccessControlSystem.UnitTests;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.UnitTests.Handlers.SlotsHandler.Contexts;

public class CreateOrUpdateSlotsHandlerTestsContext : UnitTestsContextBase
{
    private readonly SqlDbContext _dbContext;

    public CreateOrUpdateSlotsHandlerTestsContext()
    {
        _dbContext = SqlDbContextFactory.Create();
        Handler = new CreateOrUpdateSlotsHandler(_dbContext);
    }

    public CreateOrUpdateSlotsHandler Handler { get; set; }

    public async Task WithDeviceAndSlot(AccessControlDevice device)
    {
        await _dbContext.Devices.AddRangeAsync(device);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }
}
