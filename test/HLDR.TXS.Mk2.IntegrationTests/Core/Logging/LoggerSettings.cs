using AccessControlSystem.IntegrationTests.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AccessControlSystem.IntegrationTests.Core.Logging;

public static class LoggerSettings
{
    static LoggerSettings()
    {
        var fileConfiguration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(ConfigKey.AppSettingsFile)
            .Build();

        SelectedLogLevel = (LogLevel)Enum.Parse(typeof(LogLevel), fileConfiguration[ConfigKey.LogLevel] ?? LogLevel.Information.ToString());
    }

    public static LogLevel SelectedLogLevel { get; }
}