namespace NanoKontrolVolume;

public class LedChangedEvent
{
    public MidiHandler.ControlID Led { get; }
    public bool IsOn { get; }

    public LedChangedEvent(MidiHandler.ControlID led, bool isOn)
    {
        Led = led;
        IsOn = isOn;
    }
}