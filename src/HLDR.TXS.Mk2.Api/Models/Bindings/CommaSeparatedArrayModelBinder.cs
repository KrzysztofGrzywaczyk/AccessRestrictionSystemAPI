
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.Models.Bindings;

public class CommaSeparatedArrayModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

        if (valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

        var value = valueProviderResult.FirstValue;

        if (string.IsNullOrEmpty(value))
        {
            return Task.CompletedTask;
        }

        var elementType = bindingContext.ModelType.GetElementType();
        var converter = elementType != null ? TypeDescriptor.GetConverter(elementType) : null;
        var values = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        var typedValues = elementType != null ? Array.CreateInstance(elementType, values.Length) : null;
        if (typedValues != null)
        {
            for (int i = 0; i < values.Length; i++)
            {
                typedValues.SetValue(converter?.ConvertFromString(values[i].Trim()), i);
            }
        }

        bindingContext.Result = ModelBindingResult.Success(typedValues);

        return Task.CompletedTask;
    }
}
