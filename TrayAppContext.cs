using System.Diagnostics;
using Serilog;
namespace NanoKontrolVolume;

public class TrayAppContext : ApplicationContext
{
    private MainHandler mainHandler = new();

    private NotifyIcon trayIcon;

    public TrayAppContext()
    {
        trayIcon = new NotifyIcon()
        {
            Icon = new Icon("resources/NanoKontrolVolume.ico"),
            Text = "NanoKontrolVolume",
            ContextMenuStrip = new ContextMenuStrip()
            {
                Items = {
                    new ToolStripMenuItem("Open config folder", null, (s, e) => {
                        var configPath = Path.Combine(Directory.GetCurrentDirectory(), "config");
                        if (!Directory.Exists(configPath))
                        {
                            Log.Error($"Config folder does not exist at: {configPath}");
                            return;
                        }
                        Process.Start(new ProcessStartInfo("explorer.exe", configPath) { UseShellExecute = true });
                    }),
                    new ToolStripMenuItem("Restart", null, (s, e) => {
                        Log.Information($"NanoKontrolVolume restarting at: {DateTime.Now}");
                        mainHandler.Shutdown();
                        Application.Restart();
                        Environment.Exit(0);
                    }),
                    new ToolStripMenuItem("Exit", null, Exit)
                    }
            },
            Visible = true
        };
        Start();
    }

    void Start()
    {
        Log.Information($"NanoKontrolVolume started at: {DateTime.Now}");
        mainHandler.Start();
    }

    void Exit(object? sender, EventArgs e)
    {
        Log.Information($"NanoKontrolVolume stopping at: {DateTime.Now}");
        mainHandler.Shutdown();

        trayIcon.Visible = false;
        Application.Exit();
    }
}