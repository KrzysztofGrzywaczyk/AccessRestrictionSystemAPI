
using System;

namespace AccessControlSystem.SharedKernel.Logging;

#pragma warning disable SA1507 // Code should not contain multiple blank lines in a row
public interface ILogger : Microsoft.Extensions.Logging.ILogger
{
    void LogCritical(string message, params object[] args);

    void LogCritical(string message, Func<object[]> argsFormatter);

    void LogCritical(Exception exception, string message, params object[] args);

    void LogCritical(Exception exception, string message, Func<object[]> argsFormatter);

    void LogCritical(Exception exception);


    void LogError(string message, params object[] args);

    void LogError(string message, Func<object[]> argsFormatter);

    void LogError(Exception exception, string message, params object[] args);

    void LogError(Exception exception, string message, Func<object[]> argsFormatter);

    void LogError(Exception exception);


    void LogWarning(string message, params object[] args);

    void LogWarning(string message, Func<object[]> argsFormatter);

    void LogWarning(Exception exception, string message, params object[] args);

    void LogWarning(Exception exception, string message, Func<object[]> argsFormatter);


    void LogTrace(string message, params object[] args);

    void LogTrace(string message, Func<object[]> argsFormatter);

    void LogTrace(Exception exception, string message, params object[] args);

    void LogTrace(Exception exception, string message, Func<object[]> argsFormatter);


    void LogInformation(string message, params object[] args);

    void LogInformation(string message, Func<object[]> argsFormatter);

    void LogInformation(Exception exception, string message, params object[] args);

    void LogInformation(Exception exception, string message, Func<object[]> argsFormatter);


    void LogDebug(string message, params object[] args);

    void LogDebug(string message, Func<object[]> argsFormatter);

    void LogDebug(Exception exception, string message, params object[] args);

    void LogDebug(Exception exception, string message, Func<object[]> argsFormatter);
}

public interface ILogger<out T> : Microsoft.Extensions.Logging.ILogger<T>, ILogger
{
}
#pragma warning restore SA1507 // Code should not contain multiple blank lines in a row