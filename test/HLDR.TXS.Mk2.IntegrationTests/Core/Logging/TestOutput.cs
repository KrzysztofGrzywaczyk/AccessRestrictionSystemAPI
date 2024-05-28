namespace AccessControlSystem.IntegrationTests.Core.Logging;

public class TestOutput : ILogOutput
{
    private readonly ITestOutputHelper testOutputHelper;

    public TestOutput(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
    }

    public void WriteLine(string message)
    {
        testOutputHelper.WriteLine(message);
    }
}