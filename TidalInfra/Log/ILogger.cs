using System;

namespace TidalInfra.Log
{
    public interface ILogger
    {
        void WriteInfo(string message);
        void WriteError(string message);
    }

    public enum LogLevel
    {
        Info,
        Error
    }
}
