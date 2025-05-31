namespace NanoKontrolVolume;

public class ButtonHandler
{
    public event EventHandler<LedChangedEvent>? LedStatusChanged;
    public event EventHandler<ChangeApplicationEvent>? ChangeApplicationOnSlider;
    public event EventHandler<MuteApplicationEvent>? MuteApplicationOnSlider;
    public void OnButtonPress(object? sender, ButtonPressEvent e)
    {
        Task.Run(() =>
        {
            if (IsSoloButton(e.Button) && e.IsPressed)
            {
                ChangeApplicationOnSlider?.Invoke(this, new ChangeApplicationEvent(e.Button, e.Group));
            }
            else if (IsMuteButton(e.Button) && e.IsPressed)
            {
                MuteApplicationOnSlider?.Invoke(this, new MuteApplicationEvent(e.Button, e.Group));
            }
        });
    }

    public bool IsSoloButton(MidiHandler.ControlID button)
    {
        return button switch
        {
            MidiHandler.ControlID.Group1Solo or
            MidiHandler.ControlID.Group2Solo or
            MidiHandler.ControlID.Group3Solo or
            MidiHandler.ControlID.Group4Solo or
            MidiHandler.ControlID.Group5Solo or
            MidiHandler.ControlID.Group6Solo or
            MidiHandler.ControlID.Group7Solo or
            MidiHandler.ControlID.Group8Solo => true,
            _ => false,
        };
    }

    public bool IsMuteButton(MidiHandler.ControlID button)
    {
        return button switch
        {
            MidiHandler.ControlID.Group1Mute or
            MidiHandler.ControlID.Group2Mute or
            MidiHandler.ControlID.Group3Mute or
            MidiHandler.ControlID.Group4Mute or
            MidiHandler.ControlID.Group5Mute or
            MidiHandler.ControlID.Group6Mute or
            MidiHandler.ControlID.Group7Mute or
            MidiHandler.ControlID.Group8Mute => true,
            _ => false,
        };
    }
}