namespace NanoKontrolVolume;

public class DialChangedEvent : EventArgs
{
    public int Group { get; }
    public int Value { get; }

    public DialChangedEvent(int group, int value)
    {
        Group = group;
        Value = value;
    }
}