using AudioSwitcher.AudioApi.Session;

namespace NanoKontrolVolume;

public class AudioSessionObserver : IObserver<IAudioSession>
{
    private readonly Action<string> _onSessionCreated;
    public AudioSessionObserver(Action<string> onSessionCreated)
    {
        _onSessionCreated = onSessionCreated;
    }
    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(IAudioSession value)
    {
        _onSessionCreated?.Invoke(value.ExecutablePath);
    }
}

public class SessionDisconnectedObserver : IObserver<string>
{
    private readonly Action<string> _onSessionDisconnected;

    public SessionDisconnectedObserver(Action<string> onSessionDisconnected)
    {
        _onSessionDisconnected = onSessionDisconnected;
    }
    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(string value)
    {
        _onSessionDisconnected?.Invoke(value);

    }
}