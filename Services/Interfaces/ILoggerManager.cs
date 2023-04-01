namespace Service
{
    public interface ILoggerManager
    {
         void LogError(string message);
         void LogDebug(string message);
         void LogInfo(string message);
        void LogWarn(string message);
    }
}