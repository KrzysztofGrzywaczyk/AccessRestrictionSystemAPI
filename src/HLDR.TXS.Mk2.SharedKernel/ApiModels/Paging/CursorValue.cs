
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AccessControlSystem.SharedKernel.ApiModels.Paging;

public static class CursorValue
{
    public static string ConvertFrom<TCursor>(TCursor? cursor)
    {
        if (EqualityComparer<TCursor>.Default.Equals(cursor, default))
        {
            return string.Empty;
        }

        var cursorAsString = cursor!.ToString();
        var base64Bytes = Encoding.UTF8.GetBytes(cursorAsString!);
        return Convert.ToBase64String(base64Bytes);
    }

    public static TCursor? ConvertTo<TCursor>(string? value)
    {
        var cursorType = typeof(TCursor);

        if (!cursorType.IsClass)
        {
            cursorType = Nullable.GetUnderlyingType(cursorType);
            if (cursorType == null)
            {
                throw new InvalidOperationException("Cursor has to be of nullable type");
            }
        }

        if (cursorType.IsClass && !typeof(ICursor).IsAssignableFrom(cursorType))
        {
            throw new InvalidOperationException("Cursor of non-primitive nullable type has to derive from ICursor");
        }

        if (string.IsNullOrEmpty(value))
        {
            return default;
        }

        try
        {
            var base64DecodedValue = Encoding.UTF8.GetString(Convert.FromBase64String(value));

            if (typeof(ICursor).IsAssignableFrom(cursorType))
            {
                var cursor = (ICursor?)Activator.CreateInstance(cursorType);
                if (cursor is null)
                {
                    return default;
                }

                return (TCursor)cursor.FromString(base64DecodedValue);
            }

            return (TCursor)Convert.ChangeType(base64DecodedValue, cursorType, CultureInfo.InvariantCulture);
        }
        catch (FormatException)
        {
            return default!;
        }
    }
}