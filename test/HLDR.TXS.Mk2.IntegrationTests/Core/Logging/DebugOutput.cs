using System.Diagnostics;

namespace AccessControlSystem.IntegrationTests.Core.Logging;

public class DebugOutput : ILogOutput
{
    public void WriteLine(string message)
    {
        Debug.WriteLine(message);
    }
}