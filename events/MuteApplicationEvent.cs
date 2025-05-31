namespace NanoKontrolVolume;

public class MuteApplicationEvent
{
    public MidiHandler.ControlID Button { get; }
    public int Group { get; }

    public MuteApplicationEvent(MidiHandler.ControlID button, int group = 0)
    {
        Button = button;
        Group = group;
    }
}