
using AccessControlSystem.Api.Entities;
using AccessControlSystem.UnitTests;
using HLDR.TXS.Mk2.Api.Handlers.AccessControlSystems;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.UnitTests.Handlers.AccessControlSystemsHandler.Contexts;

public class GetDeviceHandlerTestsContext : UnitTestsContextBase
{
    private readonly SqlDbContext _dbContext;

    public GetDeviceHandlerTestsContext()
    {
        _dbContext = SqlDbContextFactory.Create();
        Handler = new GetDeviceHandler(_dbContext);
    }

    public GetDeviceHandler Handler { get; set; }

    public async Task WithAccessControlSystem(List<AccessControlDevice> devices)
    {
        await _dbContext.Devices.AddRangeAsync(devices);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }
}