using Avalonia;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;
using System;

namespace Flux
{
    internal class Program
    {
        public static Logger Logger = LogManager.GetCurrentClassLogger();

        [STAThread]
        public static void Main(string[] args)
        {
            // Sets the Logger.
            ColoredConsoleTarget target = new()
            {
                Layout = "${date:format=HH\\:MM\\:ss} ${logger} ${message}"
            };

            AsyncTargetWrapper asyncConsoleTarget = new(target, 10000, AsyncTargetWrapperOverflowAction.Discard);

            LoggingConfiguration config = new();
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, asyncConsoleTarget);

            LogManager.Configuration = config;

            // Instanciate Avalonia
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
    }
}
