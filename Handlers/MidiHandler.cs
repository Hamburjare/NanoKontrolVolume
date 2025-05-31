using NAudio.Midi;

namespace NanoKontrolVolume;

public class MidiHandler
{
    public event EventHandler<DialChangedEvent>? DialChanged;
    public event EventHandler<SliderChangedEvent>? SliderChanged;
    public event EventHandler<ButtonPressEvent>? ButtonPress;
    public enum ControlID : int
    {
        TrackPrevious = 0x3A,
        TrackNext = 0x3B,
        Cycle = 0x2E,
        MarkerSet = 0x3C,
        MarkerPrevious = 0x3D,
        MarkerNext = 0x3E,
        Rewind = 0x2B,
        FastForward = 0x2C,
        Stop = 0x2A,
        Play = 0x29,
        Record = 0x2D,
        Group1Dial = 0x10,
        Group1Slider = 0x00,
        Group1Solo = 0x20,
        Group1Mute = 0x30,
        Group1Record = 0x40,
        Group2Dial = Group1Dial + 1,
        Group2Slider = Group1Slider + 1,
        Group2Solo = Group1Solo + 1,
        Group2Mute = Group1Mute + 1,
        Group2Record = Group1Record + 1,
        Group3Dial = Group2Dial + 1,
        Group3Slider = Group2Slider + 1,
        Group3Solo = Group2Solo + 1,
        Group3Mute = Group2Mute + 1,
        Group3Record = Group2Record + 1,
        Group4Dial = Group3Dial + 1,
        Group4Slider = Group3Slider + 1,
        Group4Solo = Group3Solo + 1,
        Group4Mute = Group3Mute + 1,
        Group4Record = Group3Record + 1,
        Group5Dial = Group4Dial + 1,
        Group5Slider = Group4Slider + 1,
        Group5Solo = Group4Solo + 1,
        Group5Mute = Group4Mute + 1,
        Group5Record = Group4Record + 1,
        Group6Dial = Group5Dial + 1,
        Group6Slider = Group5Slider + 1,
        Group6Solo = Group5Solo + 1,
        Group6Mute = Group5Mute + 1,
        Group6Record = Group5Record + 1,
        Group7Dial = Group6Dial + 1,
        Group7Slider = Group6Slider + 1,
        Group7Solo = Group6Solo + 1,
        Group7Mute = Group6Mute + 1,
        Group7Record = Group6Record + 1,
        Group8Dial = Group7Dial + 1,
        Group8Slider = Group7Slider + 1,
        Group8Solo = Group7Solo + 1,
        Group8Mute = Group7Mute + 1,
        Group8Record = Group7Record + 1,
    }
    private MidiIn midiIn;
    private MidiOut? midiOut;
    public MidiHandler()
    {
        int deviceIndex = 0;

        for (int i = 0; i <= MidiIn.NumberOfDevices; i++)
        {
            if (MidiIn.DeviceInfo(i).ProductName == "nanoKONTROL2")
            {
                deviceIndex = i;
                break;
            }
        }

        midiIn = new MidiIn(
            deviceIndex
        );
        midiIn.MessageReceived += OnMidiMessageReceived;
        midiIn.Start();
    }

    public void StartMidiOut()
    {
        int deviceIndex = 0;

        for (int i = 0; i <= MidiOut.NumberOfDevices; i++)
        {
            if (MidiOut.DeviceInfo(i).ProductName == "nanoKONTROL2")
            {
                deviceIndex = i;
                break;
            }
        }

        midiOut = new MidiOut(deviceIndex);
    }

    public void StopMidiOut()
    {
        midiOut?.Dispose();
        midiOut = null;
    }

    public void OnLEDStatusChanged(object? sender, LedChangedEvent e)
    {
        StartMidiOut();

        int value = e.IsOn ? 127 : 0;
        midiOut?.Send(MidiMessage.ChangeControl(
            (int)e.Led,
            value,
            1
        ).RawData);

        StopMidiOut();
    }

    public void TurnOffAllLeds()
    {
        StartMidiOut();

        foreach (ControlID led in Enum.GetValues(typeof(ControlID)))
        {

            midiOut?.Send(MidiMessage.ChangeControl(
                (int)led,
                0,
                1
            ).RawData);

        }

        StopMidiOut();
    }

    public void Shutdown()
    {
        midiIn.Stop();
        midiIn.Dispose();
        StopMidiOut();
        TurnOffAllLeds();
    }

    private void OnMidiMessageReceived(object? sender, MidiInMessageEventArgs e)
    {

        var data = e.MidiEvent as ControlChangeEvent;
        if (data != null)
        {
            int controller = (int)data.Controller;
            int? dialGroup = GetDialGroup(controller);
            int? sliderGroup = GetSliderGroup(controller);

            if (dialGroup.HasValue)
            {
                DialChanged?.Invoke(this, new DialChangedEvent(dialGroup.Value, data.ControllerValue * 100 / 127));
            }
            else if (sliderGroup.HasValue)
            {
                SliderChanged?.Invoke(this, new SliderChangedEvent(sliderGroup.Value, data.ControllerValue * 100 / 127));
            }
            else
            {
                int buttonGroup = GetButtonGroup(controller);
                ButtonPress?.Invoke(this, new ButtonPressEvent((ControlID)data.Controller, data.ControllerValue > 0, buttonGroup));
            }
        }

    }

    private int? GetDialGroup(int controller)
    {
        if (controller >= (int)ControlID.Group1Dial && controller <= (int)ControlID.Group8Dial)
            return controller - (int)ControlID.Group1Dial + 1;
        return null;
    }

    private int? GetSliderGroup(int controller)
    {
        if (controller >= (int)ControlID.Group1Slider && controller <= (int)ControlID.Group8Slider)
            return controller - (int)ControlID.Group1Slider + 1;
        return null;
    }
    private int GetButtonGroup(int controller)
    {
        if (controller >= (int)ControlID.Group1Solo && controller <= (int)ControlID.Group8Solo)
            return controller - (int)ControlID.Group1Solo + 1;
        else if (controller >= (int)ControlID.Group1Mute && controller <= (int)ControlID.Group8Mute)
            return controller - (int)ControlID.Group1Mute + 1;
        else if (controller >= (int)ControlID.Group1Record && controller <= (int)ControlID.Group8Record)
            return controller - (int)ControlID.Group1Record + 1;
        return 0;
    }
}