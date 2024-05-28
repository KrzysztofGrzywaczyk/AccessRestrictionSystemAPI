using Xunit;

namespace AccessControlSystem.UnitTests;

public abstract class UnitTestsContextBase : IAsyncLifetime
{
    public virtual Task InitializeAsync() => Task.CompletedTask;

    public virtual Task DisposeAsync() => Task.CompletedTask;
}