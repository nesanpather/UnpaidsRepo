namespace Utilities.Interfaces
{
    public interface ILogger
    {
        void LogWarning(string message);
        void LogError(string message);
    }
}
