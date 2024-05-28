
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Handlers.AccessControlSystems;
using AccessControlSystem.UnitTests;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.UnitTests.Handlers.AccessControlSystemsHandler.Contexts;

public class RemoveDeviceHandlerTestsContext : UnitTestsContextBase
{
    private readonly SqlDbContext _dbContext;

    public RemoveDeviceHandlerTestsContext()
    {
        _dbContext = SqlDbContextFactory.Create();
        Handler = new RemoveDeviceHandler(_dbContext);
    }

    public RemoveDeviceHandler Handler { get; set; }

    public async Task WithAccessControlSystems(List<AccessControlDevice> devices)
    {
        await _dbContext.Devices.AddRangeAsync(devices);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task AccessControlSytemShouldNotExistInDatabase(string deviceId)
    {
        var Device = await _dbContext.Devices
            .SingleOrDefaultAsync(x => x.DeviceName == deviceId);

        Device.Should().BeNull();
    }
}
