using System;
using Utilities.Interfaces;

namespace Utilities
{
    public sealed class ConsoleLogger : ILogger
    {
        static int _instanceCounter;
        private static readonly Lazy<ConsoleLogger> singleInstance = new Lazy<ConsoleLogger>(() => new ConsoleLogger());
        private ConsoleLogger()
        {
        }
        public static ConsoleLogger SingleInstance => singleInstance.Value;

        public void LogWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Warning: {0}", message);
            Console.ResetColor();
        }

        public void LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: {0} ", message);
            Console.ResetColor();
        }
    }
}
