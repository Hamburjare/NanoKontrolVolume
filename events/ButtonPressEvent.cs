namespace NanoKontrolVolume;

public class ButtonPressEvent
{
    public MidiHandler.ControlID Button { get; }
    public bool IsPressed { get; }
    public int Group { get; }
    
    public ButtonPressEvent(MidiHandler.ControlID button, bool isPressed, int group = 0)
    {
        Button = button;
        IsPressed = isPressed;
        Group = group;
    }
}