
using System.Diagnostics;
using System.Runtime.InteropServices;
using AudioSwitcher.AudioApi.CoreAudio;
using Newtonsoft.Json.Linq;

namespace NanoKontrolVolume;


public class VolumeHandler
{

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    [DllImport("user32.dll")]
    private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

    private const byte VK_VOLUME_UP = 0xAF;
    private const byte VK_VOLUME_DOWN = 0xAE;
    private const uint KEYEVENTF_KEYUP = 0x0002;

    private int prevDefaultPlaybackVolume = 0;

    private readonly CoreAudioController audioController = new CoreAudioController();
    private readonly CoreAudioDevice defaultPlaybackDevice;

    Dictionary<int, string> sliderMappings = new Dictionary<int, string>();
    Dictionary<int, string> dialMappings = new Dictionary<int, string>();

    public event EventHandler<LedChangedEvent>? LedStatusChanged;

    public VolumeHandler()
    {
        defaultPlaybackDevice = audioController.DefaultPlaybackDevice;
        defaultPlaybackDevice.SessionController.SessionCreated.Subscribe(new AudioSessionObserver(OnSessionCreated));

        defaultPlaybackDevice.SessionController.SessionDisconnected.Subscribe(new SessionDisconnectedObserver(OnSessionDisconnected));


        LoadMappings();
    }

    public void Shutdown()
    {
        SaveMappings();
        TurnOffLeds();
    }

    private void OnSessionDisconnected(string sessionId)
    {
        CheckProcessIdInSliderMappings(sessionId, false);
    }

    private void OnSessionCreated(string sessionId)
    {
        CheckProcessIdInSliderMappings(sessionId, true);
    }

    private void LoadMappings()
    {
        try
        {
            string json = File.ReadAllText(@"config/groupMappings.json");
            var jObj = JObject.Parse(json);
            foreach (var group in jObj)
            {
                // Extract group number from key like "Group1"
                if (group.Key.StartsWith("Group") && int.TryParse(group.Key.Substring(5), out int groupNum))
                {
                    var sliderValue = group.Value?["Slider"]?.ToString();
                    if (!string.IsNullOrEmpty(sliderValue))
                    {
                        sliderMappings[groupNum] = sliderValue;
                    }
                    var dialValue = group.Value?["Dial"]?.ToString();
                    if (!string.IsNullOrEmpty(dialValue))
                    {
                        dialMappings[groupNum] = dialValue;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading mappings: {ex.Message}");
        }
    }

    private void SaveMappings()
    {
        try
        {
            var jObj = new JObject();
            for (int i = 1; i <= 8; i++)
            {
                jObj[$"Group{i}"] = new JObject
                {
                    ["Dial"] = dialMappings.TryGetValue(i, out string? dialValue) ? dialValue : "",
                    ["Slider"] = sliderMappings.TryGetValue(i, out string? sliderValue) ? sliderValue : ""
                };
            }
            File.WriteAllText(@"config/groupMappings.json", jObj.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving mappings: {ex.Message}");
        }
    }


    public void OnSliderChanged(object? sender, SliderChangedEvent e)
    {
        if (!sliderMappings.TryGetValue(e.Group, out string? appId))
        {
            return;
        }

        if (appId.Equals("Default Output Device", StringComparison.OrdinalIgnoreCase))
        {
            defaultPlaybackDevice.Volume = e.Value;
            if (defaultPlaybackDevice.Volume != prevDefaultPlaybackVolume)
            {
                ShowVolumeOSD();
            }
            prevDefaultPlaybackVolume = (int)defaultPlaybackDevice.Volume;
            return;
        }

        foreach (var session in defaultPlaybackDevice.SessionController.All())
        {
            if (session.Id.Equals(appId, StringComparison.OrdinalIgnoreCase))
            {
                session.Volume = e.Value;
            }
        }
    }

    public void OnDialChanged(object? sender, DialChangedEvent e)
    {
        if (!dialMappings.TryGetValue(e.Group, out string? appId))
        {
            return;
        }

        foreach (var session in defaultPlaybackDevice.SessionController.All())
        {
            if (session.Id.Equals(appId, StringComparison.OrdinalIgnoreCase))
            {
                session.Volume = e.Value;
            }
        }
    }

    public void OnChangeApplicationOnSlider(object? sender, ChangeApplicationEvent e)
    {
        if (sliderMappings.TryGetValue(e.Group, out string? app))
        {
            sliderMappings.Remove(e.Group);
            LedStatusChanged?.Invoke(this, new LedChangedEvent(e.Button, false));
            LedStatusChanged?.Invoke(this, new LedChangedEvent((MidiHandler.ControlID)Enum.Parse(typeof(MidiHandler.ControlID), $"Group{e.Group}Record"), false));
        }
        else
        {
            app = GetForegroundProcessName();
            if (app != null)
            {
                sliderMappings.Add(e.Group, app);
                LedStatusChanged?.Invoke(this, new LedChangedEvent(e.Button, true));
                GoThroughActiveSessions(e.Group, app);
            }
        }
        SaveMappings();
    }

    public void OnMuteApplicationOnSlider(object? sender, MuteApplicationEvent e)
    {
        if (sliderMappings.TryGetValue(e.Group, out string? appId))
        {
            foreach (var session in defaultPlaybackDevice.SessionController.All())
            {
                if (session.Id.Equals(appId, StringComparison.OrdinalIgnoreCase))
                {
                    session.IsMuted = !session.IsMuted;
                    LedStatusChanged?.Invoke(this, new LedChangedEvent(e.Button, session.IsMuted));
                }
            }
        }
    }


    public string? GetForegroundProcessName()
    {
        var hwnd = GetForegroundWindow();
        if (hwnd == IntPtr.Zero)
            return null;

        GetWindowThreadProcessId(hwnd, out uint pid);
        var proc = Process.GetProcessById((int)pid);
        var sessions = defaultPlaybackDevice.SessionController.All();
        foreach (var session in sessions)
        {
            if (session.ProcessId == proc.Id)
            {
                return session.Id;
            }
        }
        return null;
    }

    private void CheckProcessIdInSliderMappings(string sessionId, bool LedOn)
    {
        foreach (var group in sliderMappings)
        {
            if (group.Value.Equals(sessionId, StringComparison.OrdinalIgnoreCase))
            {
                LedStatusChanged?.Invoke(this, new LedChangedEvent((MidiHandler.ControlID)Enum.Parse(typeof(MidiHandler.ControlID), $"Group{group.Key}Record"), LedOn));
                return;
            }
        }
    }

    private void GoThroughActiveSessions(int group, string processId)
    {
        if (processId.Equals("Default Output Device", StringComparison.OrdinalIgnoreCase))
        {
            // If the process ID is "Default Output Device", turn on the Record LED for the group
            LedStatusChanged?.Invoke(this, new LedChangedEvent((MidiHandler.ControlID)Enum.Parse(typeof(MidiHandler.ControlID), $"Group{group}Record"), true));
            return;
        }
        var sessions = defaultPlaybackDevice.SessionController.All();
        foreach (var session in sessions)
        {
            // Check if the session ID matches the process ID and turn on the Record LED
            if (session.Id.Equals(processId, StringComparison.OrdinalIgnoreCase))
            {
                LedStatusChanged?.Invoke(this, new LedChangedEvent((MidiHandler.ControlID)Enum.Parse(typeof(MidiHandler.ControlID), $"Group{group}Record"), true));
                break;
            }
            // Check if the session's ExecutablePath or DisplayName matches the process ID
            else if ((session.ExecutablePath != null && session.ExecutablePath.Contains(processId, StringComparison.OrdinalIgnoreCase)) || (session.DisplayName != null && session.DisplayName.Contains(processId, StringComparison.OrdinalIgnoreCase)))
            {
                // If it matches, update the sliderMappings dictionary to include the session ID
                sliderMappings[group] = session.Id;

                // Turn on the Record LED for the group
                LedStatusChanged?.Invoke(this, new LedChangedEvent((MidiHandler.ControlID)Enum.Parse(typeof(MidiHandler.ControlID), $"Group{group}Record"), true));
                SaveMappings();
                break;
            }
        }

    }

    public void GoThroughAllSliderMappings()
    {
        TurnOffLeds();
        foreach (var group in sliderMappings)
        {
            // Turn on the Solo LED for the group
            LedStatusChanged?.Invoke(this, new LedChangedEvent((MidiHandler.ControlID)Enum.Parse(typeof(MidiHandler.ControlID), $"Group{group.Key}Solo"), true));

            // Check if the process is active and turn on the Record LED
            GoThroughActiveSessions(group.Key, group.Value);
        }
    }

    public void TurnOffLeds()
    {
        foreach (var group in sliderMappings)
        {
            LedStatusChanged?.Invoke(this, new LedChangedEvent((MidiHandler.ControlID)Enum.Parse(typeof(MidiHandler.ControlID), $"Group{group.Key}Solo"), false));

            LedStatusChanged?.Invoke(this, new LedChangedEvent((MidiHandler.ControlID)Enum.Parse(typeof(MidiHandler.ControlID), $"Group{group.Key}Record"), false));
        }
    }

    private void ShowVolumeOSD()
    {
        // Simulate volume up
        keybd_event(VK_VOLUME_UP, 0, 0, 0);
        keybd_event(VK_VOLUME_UP, 0, KEYEVENTF_KEYUP, 0);
        // Simulate volume down to restore original volume
        keybd_event(VK_VOLUME_DOWN, 0, 0, 0);
        keybd_event(VK_VOLUME_DOWN, 0, KEYEVENTF_KEYUP, 0);
    }
}