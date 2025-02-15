using Microsoft.Extensions.Logging;
using Serilog;

namespace MovieVault.Core.Configuration
{
    public static class LoggingConfig
    {
        private static readonly ILoggerFactory _loggerFactory;

        static LoggingConfig()
        {
            var logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MovieVault", "logs", "application.log");
            Directory.CreateDirectory(Path.GetDirectoryName(logPath)!);

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)
                .CreateLogger();

            _loggerFactory = LoggerFactory.Create(builder => builder.AddSerilog());
        }

        public static ILogger<T> GetLogger<T>()
        {
            return _loggerFactory.CreateLogger<T>();
        }

        public static ILoggerFactory GetLoggerFactory()
        {
            return _loggerFactory;
        }
    }
}