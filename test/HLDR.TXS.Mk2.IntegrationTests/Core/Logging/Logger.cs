using Microsoft.Extensions.Logging;
using System.Text;

namespace AccessControlSystem.IntegrationTests.Core.Logging;

public class Logger
{
    private readonly LogLevel _selectedLogLevel;

    private readonly ILogOutput _output;

    public Logger(ILogOutput? logOutput)
    {
        _selectedLogLevel = LoggerSettings.SelectedLogLevel;
        _output = logOutput ?? new DebugOutput();
    }

    public void Debug(object o, string message, params object[] parameters)
    {
        LogMessage(LogLevel.Debug, o, null!, message, parameters);
    }

    public void Info(object o, string message, params object[] parameters)
    {
        LogMessage(LogLevel.Information, o, null!, message, parameters);
    }

    public void Error(object o, Exception e)
    {
        LogMessage(LogLevel.Error, o, e, null!);
    }

    public void Error(object o, Exception e, string message, params object[] parameters)
    {
        LogMessage(LogLevel.Error, o, e, message, parameters);
    }

    public void Fatal(object o, string message, params object[] parameters)
    {
        LogMessage(LogLevel.Error, o, null, message, parameters);
        throw new TestFrameworkException(message);
    }

    public void Fatal(object o, Exception e, string message, params object[] parameters)
    {
        LogMessage(LogLevel.Error, o, e, message, parameters);
        throw new TestFrameworkException(message, e);
    }

    private void LogMessage(LogLevel logLevel, object? o, Exception? e, string? message, params object[] parameters)
    {
        if (logLevel < _selectedLogLevel)
        {
            return;
        }

        var messageBuilder = new StringBuilder("[")
            .Append(DateTime.Now.ToString("dd.MM.yyyy_hh:mm:ss.fff"))
            .Append("] > [")
            .Append($"Thread-{Thread.CurrentThread.ManagedThreadId}")
            .Append("] > [")
            .Append(logLevel.ToString())
            .Append("] > ");

        if (o != null)
        {
            messageBuilder.Append("\"")
                .Append(o.GetType().Name)
                .Append("\" > ");
        }

        if (message != null)
        {
            messageBuilder.Append(parameters.Length > 0 ? string.Format(message, parameters) : message);

            if (e != null)
            {
                messageBuilder.Append(Environment.NewLine);
            }
        }

        if (e != null)
        {
            messageBuilder.Append(e.ToString());
        }

        _output.WriteLine(messageBuilder.ToString());
    }
}