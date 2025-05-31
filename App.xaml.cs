using System.Configuration;
using System.Data;
using System.Windows;

namespace NanoKontrolVolume;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{

    MainHandler mainHandler = new();

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        mainHandler.Shutdown();
        base.OnExit(e);
    }

}

