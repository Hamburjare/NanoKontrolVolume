namespace NanoKontrolVolume;

public class MainHandler
{
    public MidiHandler midiHandler = new();
    public VolumeHandler volumeHandler = new();
    public ButtonHandler buttonHandler = new();

    public MainHandler()
    {
        midiHandler.SliderChanged += volumeHandler.OnSliderChanged;
        midiHandler.DialChanged += volumeHandler.OnDialChanged;
        midiHandler.ButtonPress += buttonHandler.OnButtonPress;
    }
}