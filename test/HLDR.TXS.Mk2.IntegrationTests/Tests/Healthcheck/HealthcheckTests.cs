using AccessControlSystem.IntegrationTests.Core;
using System.Net;

namespace AccessControlSystem.IntegrationTests.Tests.Healthcheck;

[Collection(nameof(AssemblyCollection))]
public class HealthcheckTests : TestsBase<HealthcheckContext, AssemblySharedFixture>
{
    public HealthcheckTests(ITestOutputHelper outputHelper, AssemblySharedFixture fixture)
        : base(outputHelper, fixture)
    {
    }

    [Fact]
    public async Task GivenEmptyRequest_WhenHealthcheckIsQueried_ShouldReturnOkStatus()
    {
        await Context.WhenQueriedHealthcheck();
        await Context.ThenStatusCodeIsReturned(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GivenEmptyRequest_WhenHealthcheckWithNoDependenciesIsQueried_ShouldReturnOkStatus()
    {
        await Context.WhenQueriedHealthcheckWithNoDependencies();
        await Context.ThenStatusCodeIsReturned(HttpStatusCode.OK);
    }
}