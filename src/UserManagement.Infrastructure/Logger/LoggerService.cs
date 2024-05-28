using NLog;

namespace UserManagement.Infrastructure.Logger;

public class LoggerService : ILoggerService
{
    private static ILogger logger = LogManager.GetCurrentClassLogger();

    public void LogDebug(Exception? exception, string message)
    {
        logger.Debug(exception, message);
    }

    public void LogError(Exception? exception, string message)
    {
        logger.Error(exception, message);
    }

    public void LogInfo(Exception? exception, string message)
    {
        logger.Info(exception, message);
    }

    public void LogWarn(Exception? exception, string message)
    {
        logger.Warn(exception, message);
    }
    
    public void LogTrace(Exception? exception, string message)
    {
        logger.Trace(exception, message);
    }
}