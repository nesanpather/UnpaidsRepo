namespace Utilities.Interfaces
{
    public interface ISettings
    {
        string this[string key] { get; }
    }
}
