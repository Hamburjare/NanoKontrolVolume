namespace NanoKontrolVolume;

public class SliderChangedEvent : EventArgs
{
    public int Group { get; }
    public int Value { get; }

    public SliderChangedEvent(int group, int value)
    {
        Group = group;
        Value = value;
    }
}