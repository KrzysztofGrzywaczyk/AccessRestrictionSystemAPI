using AccessControlSystem.IntegrationTests.Core.Logging;
using AutoFixture;

namespace AccessControlSystem.IntegrationTests.Core;

#pragma warning disable SA1649
#pragma warning disable SA1402
public abstract class TestsBase : IAsyncLifetime
{
    protected TestsBase(ITestOutputHelper outputHelper)
    {
        Logger = new Logger(new TestOutput(outputHelper));
        Fixture = new Fixture();
        Fixture.Register(() => Logger);
    }

    protected Logger Logger { get; }

    protected Fixture Fixture { get; }

    public virtual Task InitializeAsync() => Task.CompletedTask;

    public virtual Task DisposeAsync() => Task.CompletedTask;
}

public abstract class TestsBase<TContext> : TestsBase
    where TContext : IAsyncLifetime
{
    protected TestsBase(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
        Context = Fixture.Create<TContext>();
    }

    protected TContext Context { get; }

    public override Task InitializeAsync() => Context.InitializeAsync();

    public override async Task DisposeAsync()
    {
        await Context.DisposeAsync();
        await base.DisposeAsync();
    }
}

public abstract class TestsBase<TContext, TFixture> : TestsBase
    where TContext : IAsyncLifetime
{
    protected TestsBase(ITestOutputHelper outputHelper, TFixture fixture)
        : base(outputHelper)
    {
        Fixture.Register(() => fixture);
        Context = Fixture.Create<TContext>();
    }

    protected TContext Context { get; }

    public override Task InitializeAsync() => Context.InitializeAsync();

    public override async Task DisposeAsync()
    {
        await Context.DisposeAsync();
        await base.DisposeAsync();
    }
}

public abstract class TestsBase<TContext, TFixture1, TFixture2> : TestsBase
    where TContext : IAsyncLifetime
{
    protected TestsBase(ITestOutputHelper outputHelper, TFixture1 fixture1, TFixture2 fixture2)
        : base(outputHelper)
    {
        Fixture.Register(() => fixture1);
        Fixture.Register(() => fixture2);
        Context = Fixture.Create<TContext>();
    }

    protected TContext Context { get; }

    public override Task InitializeAsync() => Context.InitializeAsync();

    public override async Task DisposeAsync()
    {
        await Context.DisposeAsync();
        await base.DisposeAsync();
    }
}
#pragma warning restore SA1402
#pragma warning restore SA1649
