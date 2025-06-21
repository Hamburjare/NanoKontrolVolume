namespace NanoKontrolVolume;

public class MainHandler
{
    public MidiHandler midiHandler = new();
    public VolumeHandler volumeHandler = new();
    public ButtonHandler buttonHandler = new();

    public MainHandler() { }

    public void Start()
    {
        midiHandler.SliderChanged += volumeHandler.OnSliderChanged;
        midiHandler.DialChanged += volumeHandler.OnDialChanged;
        midiHandler.ButtonPress += buttonHandler.OnButtonPress;
        buttonHandler.LedStatusChanged += midiHandler.OnLEDStatusChanged;
        buttonHandler.ChangeApplicationOnSlider += volumeHandler.OnChangeApplicationOnSlider;
        buttonHandler.MuteApplicationOnSlider += volumeHandler.OnMuteApplicationOnSlider;
        volumeHandler.LedStatusChanged += midiHandler.OnLEDStatusChanged;

        TurnOnLedsAsync();

    }

    public void Shutdown()
    {
        volumeHandler.Shutdown();
        midiHandler.Shutdown();
        buttonHandler.Shutdown();
    }

    private async void TurnOnLedsAsync()
    {
        await Task.Delay(2000);
        volumeHandler.GoThroughAllSliderMappings();
        buttonHandler.TurnButtonOnLeds();
    }
}