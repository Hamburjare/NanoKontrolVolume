
using Serilog;

namespace NanoKontrolVolume;

public class Worker : BackgroundService
{
    private MainHandler mainHandler = new();

    public Worker() { }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Log.Information($"NanoKontrolVolume started at: {DateTime.Now}");
        mainHandler.Start();

        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        Log.Information($"NanoKontrolVolume stopping at: {DateTime.Now}");
        mainHandler.Shutdown();

        await base.StopAsync(cancellationToken);
    }
}
