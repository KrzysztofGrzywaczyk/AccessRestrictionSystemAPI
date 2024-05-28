
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Handlers.AccessControlSystems;
using AccessControlSystem.UnitTests;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.UnitTests.Handlers.AccessControlSystemsHandler.Contexts;

public class CreateOrModifyDeviceHandlerTestsContext : UnitTestsContextBase
{
    private readonly SqlDbContext _dbContext;

    public CreateOrModifyDeviceHandlerTestsContext()
    {
        _dbContext = SqlDbContextFactory.Create();
        Handler = new CreateOrModifyDeviceHandler(_dbContext);
    }

    public CreateOrModifyDeviceHandler Handler { get; set; }

    public async Task WithDevice(List<AccessControlDevice> devices)
    {
        await _dbContext.Devices.AddRangeAsync(devices);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task DeviceShouldBeAddedToDatabase(string deviceId)
    {
        var device = await _dbContext.Devices
            .SingleOrDefaultAsync(x => x.DeviceName == deviceId);

        device.Should().NotBeNull();
    }
}
