using Azure.Identity;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace AccessControlSystem.IntegrationTests.Core.Configuration;

public class TestsConfiguration
{
    private readonly IConfigurationRoot configuration;

    public TestsConfiguration()
    {
        var fileConfiguration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(ConfigKey.AppSettingsFile)
            .Build();

        var keyVault = fileConfiguration.GetValue<string>(ConfigKey.KeyVault);

        configuration = new ConfigurationBuilder()
           .AddAzureKeyVault(
                new Uri(keyVault!),
                new DefaultAzureCredential(new DefaultAzureCredentialOptions()))
            .AddConfiguration(fileConfiguration)
            .Build();
    }

    public string GetProperty(string propertyPath)
    {
        var property = configuration[propertyPath];
        return property!;
    }

    public TConfig Get<TConfig>(string sectionName) => configuration.GetSection(sectionName).Get<TConfig>() ?? throw new ConfigurationErrorsException($"Not found section {sectionName} in configuration.");
}