using Serilog;

namespace NanoKontrolVolume;

static class Program
{

    [STAThread]
    static void Main()
    {
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("logs/log.log")
            .WriteTo.Console()
            .CreateLogger();

        if (!OperatingSystem.IsWindows())
        {
            Log.Fatal("This application is only supported on Windows.");
            return;
        }

        AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
        {
            Log.Fatal(e.ExceptionObject as Exception, "Unhandled exception occurred");
        };

        Log.Information("Starting NanoKontrolVolume...");

        Application.Run(new TrayAppContext());

        
    }
}