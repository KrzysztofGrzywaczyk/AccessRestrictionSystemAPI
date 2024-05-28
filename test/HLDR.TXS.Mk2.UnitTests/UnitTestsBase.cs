using Xunit;

namespace AccessControlSystem.UnitTests;

public abstract class UnitTestsBase<T> : IAsyncLifetime
    where T : IAsyncLifetime, new()
{
    protected T Context { get; } = new();

    public Task InitializeAsync() => Context.InitializeAsync();

    public Task DisposeAsync() => Context.DisposeAsync();
}