namespace UserManagement.Infrastructure.Logger;

public interface ILoggerService
{
    void LogInfo(Exception? exception, string message);
    void LogWarn(Exception? exception, string message);
    void LogDebug(Exception? exception, string message);
    void LogError(Exception? exception, string message);
    void LogTrace(Exception? exception, string message);
}