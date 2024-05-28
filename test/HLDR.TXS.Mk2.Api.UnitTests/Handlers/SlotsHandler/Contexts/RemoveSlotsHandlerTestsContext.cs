
using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Handlers.Slots;
using AccessControlSystem.UnitTests;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.UnitTests.Handlers.SlotsHandler.Contexts;

public class RemoveSlotsHandlerTestsContext : UnitTestsContextBase
{
    private readonly SqlDbContext _dbContext;

    public RemoveSlotsHandlerTestsContext()
    {
        _dbContext = SqlDbContextFactory.Create();
        Handler = new RemoveSlotsHandler(_dbContext);
    }

    public RemoveSlotsHandler Handler { get; set; }

    public async Task WithDeviceWithSlotCreated(AccessControlDevice device)
    {
        await _dbContext.Devices.AddAsync(device);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task SlotsShouldNotExistInDatabase(List<string> slotNames)
    {
        var slots = await _dbContext.Slots.Where(s => slotNames.Contains(s.SlotName!)).ToListAsync();
        slots.Should().BeNullOrEmpty();
    }
}
