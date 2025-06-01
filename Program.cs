using NanoKontrolVolume;
using Serilog;

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

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
