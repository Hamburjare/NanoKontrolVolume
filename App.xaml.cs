using System.Configuration;
using System.Data;
using System.Windows;
using AudioSwitcher.AudioApi.CoreAudio;

namespace NanoKontrolVolume;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    
    public MidiHandler midiHandler;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        midiHandler = new MidiHandler(0);

        var defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
        foreach (var session in defaultPlaybackDevice.SessionController.All())
        {
            if (session.DisplayName == "Spotify")
            {
                session.Volume = 55;
            }
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
    }

}

