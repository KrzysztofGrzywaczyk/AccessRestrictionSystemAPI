
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;

namespace AccessControlSystem.SharedKernel.Logging;

/// <summary>
/// Implementation of a wrapper for MSFT ILogger interface is here for two main reasons:
/// 1. Almost all of the methods below are MSFT ILogger extensions https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.loggerextensions?view=dotnet-plat-ext-7.0&viewFallbackFrom=net-6.0
///    This means that you cannot easily and clearly verify calls to these methods in tests, when mocking ILogger.
///    Check <see cref="AccessControlSystem.SharedKernel.UnitTests.Behaviors.LoggingBehaviorTests"/> for a working example.
/// 2. Each group of methods received an additional overload, accepting argsFormatter. It is a lambda, which is calculated only, when a particular LogLevel is actually enabled.
///    Example:
///    For certain scenarios, especially when gathering debug logs, we need verbose information, which may require compute intensive tasks (such as ie. serialization).
///    However, debug logs are usually off for the majority of application lifetime.
///    This additional overload with lambda allows for disabling computing arguments, when the logging itself wouldn't actually take place anyways.
/// </summary>
/// <typeparam name="T">Logger category.</typeparam>
[ExcludeFromCodeCoverage]
#pragma warning disable SA1507 // Code should not contain multiple blank lines in a row
#pragma warning disable SA1402 // File may only contain single line
public class Logger : ILogger
{
    private readonly Microsoft.Extensions.Logging.ILogger _internalLogger;

    public Logger(ILoggerFactory loggerFactory, string categoryName)
    {
        _internalLogger = loggerFactory.CreateLogger(categoryName);
    }

    protected internal Logger(Microsoft.Extensions.Logging.ILogger internalLogger)
    {
        _internalLogger = internalLogger;
    }

    public void LogCritical(string message, params object[] args) => _internalLogger.LogCritical(message, args);

    public void LogCritical(string message, Func<object[]> argsFormatter) => LogMessage(LogLevel.Critical, message, argsFormatter, LogCritical);

    public void LogCritical(Exception exception, string message, params object[] args) => _internalLogger.LogCritical(exception, message, args);

    public void LogCritical(Exception exception, string message, Func<object[]> argsFormatter) => LogException(LogLevel.Critical, exception, message, argsFormatter, LogCritical);

    public void LogCritical(Exception exception) => _internalLogger.LogCritical(exception, exception.Message);


    public void LogError(string message, params object[] args) => _internalLogger.LogError(message, args);

    public void LogError(string message, Func<object[]> argsFormatter) => LogMessage(LogLevel.Error, message, argsFormatter, LogError);

    public void LogError(Exception exception, string message, params object[] args) => _internalLogger.LogError(exception, message, args);

    public void LogError(Exception exception, string message, Func<object[]> argsFormatter) => LogException(LogLevel.Error, exception, message, argsFormatter, LogError);

    public void LogError(Exception exception) => _internalLogger.LogError(exception, exception.Message);


    public void LogWarning(string message, params object[] args) => _internalLogger.LogWarning(message, args);

    public void LogWarning(string message, Func<object[]> argsFormatter) => LogMessage(LogLevel.Warning, message, argsFormatter, LogWarning);

    public void LogWarning(Exception exception, string message, params object[] args) => _internalLogger.LogWarning(exception, message, args);

    public void LogWarning(Exception exception, string message, Func<object[]> argsFormatter) => LogException(LogLevel.Warning, exception, message, argsFormatter, LogWarning);


    public void LogTrace(string message, params object[] args) => _internalLogger.LogTrace(message, args);

    public void LogTrace(string message, Func<object[]> argsFormatter) => LogMessage(LogLevel.Trace, message, argsFormatter, LogTrace);

    public void LogTrace(Exception exception, string message, params object[] args) => _internalLogger.LogTrace(exception, message, args);

    public void LogTrace(Exception exception, string message, Func<object[]> argsFormatter) => LogException(LogLevel.Trace, exception, message, argsFormatter, LogTrace);


    public void LogInformation(string message, params object[] args) => _internalLogger.LogInformation(message, args);

    public void LogInformation(string message, Func<object[]> argsFormatter) => LogMessage(LogLevel.Information, message, argsFormatter, LogInformation);

    public void LogInformation(Exception exception, string message, params object[] args) => _internalLogger.LogInformation(exception, message, args);

    public void LogInformation(Exception exception, string message, Func<object[]> argsFormatter) => LogException(LogLevel.Information, exception, message, argsFormatter, LogInformation);


    public void LogDebug(string message, params object[] args) => _internalLogger.LogDebug(message, args);

    public void LogDebug(string message, Func<object[]> argsFormatter) => LogMessage(LogLevel.Debug, message, argsFormatter, LogDebug);

    public void LogDebug(Exception exception, string message, params object[] args) => _internalLogger.LogDebug(exception, message, args);

    public void LogDebug(Exception exception, string message, Func<object[]> argsFormatter) => LogException(LogLevel.Debug, exception, message, argsFormatter, LogDebug);


    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) =>
        _internalLogger.Log(logLevel, eventId, state, exception, formatter);

    public bool IsEnabled(LogLevel logLevel) => _internalLogger.IsEnabled(logLevel);

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull => _internalLogger.BeginScope(state);

    private void LogMessage(
        LogLevel logLevel,
        string message,
        Func<object[]> argsFormatter,
        Action<string, object[]> loggerCall)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        var args = argsFormatter();
        loggerCall(message, args);
    }

    private void LogException(
        LogLevel logLevel,
        Exception exception,
        string message,
        Func<object[]> argsFormatter,
        Action<Exception, string, object[]> loggerCall)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        var args = argsFormatter();
        loggerCall(exception, message, args);
    }
}

public class Logger<T> : Logger, ILogger<T>
{
    public Logger(ILoggerFactory loggerFactory)
        : base(loggerFactory.CreateLogger<T>())
    {
    }
}
#pragma warning restore SA1402
#pragma warning restore SA1507 // Code should not contain multiple blank lines in a row