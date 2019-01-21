namespace Utilities.Interfaces
{
    public interface ICustomLogger
    {
        void LogWarning(string message);
        void LogError(string message);
    }
}
