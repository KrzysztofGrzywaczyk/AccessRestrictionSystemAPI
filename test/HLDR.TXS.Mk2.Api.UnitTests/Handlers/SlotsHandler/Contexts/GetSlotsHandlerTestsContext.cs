
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Handlers.Slots;
using AccessControlSystem.UnitTests;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.UnitTests.Handlers.SlotsHandler.Contexts;

public class GetSlotsHandlerTestsContext : UnitTestsContextBase
{
    private readonly SqlDbContext _dbContext;

    public GetSlotsHandlerTestsContext()
    {
        _dbContext = SqlDbContextFactory.Create();
        Handler = new GetSlotsHandler(_dbContext);
    }

    public GetSlotsHandler Handler { get; set; }

    public async Task WithDeviceAndSlotCreated(AccessControlDevice device)
    {
        await _dbContext.Devices.AddRangeAsync(device);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }
}
