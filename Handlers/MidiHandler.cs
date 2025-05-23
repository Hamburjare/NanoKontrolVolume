using NAudio.Midi;

namespace NanoKontrolVolume;

public class MidiHandler
{
    public MidiIn midiIn;
    public MidiHandler(int deviceIndex)
    {
        midiIn = new MidiIn(
            deviceIndex
        );
        midiIn.MessageReceived += OnMidiMessageReceived;
        midiIn.Start();
    }

    private void OnMidiMessageReceived(object? sender, MidiInMessageEventArgs e)
    {

        var data = e.MidiEvent as ControlChangeEvent;
        if (data != null)
        {
            Console.WriteLine($"CC#{data.Controller} = {data.ControllerValue}");
        }

    }
}