namespace NanoKontrolVolume;

public class ChangeApplicationEvent
{
    public MidiHandler.ControlID Button { get; }
    public int Group { get; }

    public ChangeApplicationEvent(MidiHandler.ControlID button, int group = 0)
    {
        Button = button;
        Group = group;
    }
}