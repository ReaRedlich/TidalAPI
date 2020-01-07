using System;

namespace TidalInfra.Log
{
    public class ConsoleLogger : BaseLogger
    {
        string dateFormat = "G";//08/04/2007 21:08:59
        public override void Write(LogLevel logLevel, string message)
        {
            Console.WriteLine($"{DateTime.UtcNow.ToString(dateFormat)} - {logLevel} : {message}");
        }
    }
}
