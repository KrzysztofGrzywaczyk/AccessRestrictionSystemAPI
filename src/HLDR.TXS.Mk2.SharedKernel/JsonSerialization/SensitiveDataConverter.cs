
using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AccessControlSystem.SharedKernel.JsonSerialization;

public class SensitiveDataConverter<T> : JsonConverter<T>
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => JsonSerializer.Deserialize<T>(ref reader, options);

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            return;
        }

        writer.WriteStartObject();

        var propertiesWithoutSensitiveData = value
            .GetType()
            .GetProperties().Where(pInfo => pInfo.GetCustomAttribute<SensitiveDataAttribute>() == null);
        foreach (var property in propertiesWithoutSensitiveData)
        {
            var propValue = property.GetValue(value);
            if (propValue is not null ||
                (propValue is null && options.DefaultIgnoreCondition == JsonIgnoreCondition.Never))
            {
                writer.WritePropertyName(property.Name);
                JsonSerializer.Serialize(writer, propValue, options);
            }
        }

        writer.WriteEndObject();
    }
}