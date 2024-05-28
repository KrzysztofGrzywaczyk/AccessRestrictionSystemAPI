
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AccessControlSystem.SharedKernel.JsonSerialization;

public static class JsonSerializerOptionsProvider
{
    static JsonSerializerOptionsProvider()
    {
        Default = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        Default.Converters.Add(new JsonStringEnumConverter());
        Default.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

        Sensitive = new JsonSerializerOptions(Default);
        Sensitive.Converters.Add(new SensitiveDataConverterFactory());
    }

    public static JsonSerializerOptions Default { get; }

    public static JsonSerializerOptions Sensitive { get; }
}