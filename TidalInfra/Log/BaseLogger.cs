namespace TidalInfra.Log
{
    public abstract class BaseLogger : ILogger
    {
        public abstract void Write(LogLevel logLevel, string message);

        public void WriteInfo(string message)
        {
            Write(LogLevel.Info, message);
        }

        public void WriteError(string message)
        {
            Write(LogLevel.Error, message);
        }
    }
}
