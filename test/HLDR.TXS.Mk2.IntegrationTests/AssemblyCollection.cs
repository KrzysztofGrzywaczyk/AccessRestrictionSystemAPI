namespace AccessControlSystem.IntegrationTests;

[CollectionDefinition(nameof(AssemblyCollection))]
public class AssemblyCollection : ICollectionFixture<AssemblySharedFixture>
{
    //// https://xunit.net/docs/shared-context
}