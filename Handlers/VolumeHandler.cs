
using System.Diagnostics;
using System.IO;
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

    private void OnSessionDisconnected(string sessionPath)
    {
        string? processName = Path.GetFileNameWithoutExtension(sessionPath);
        Console.WriteLine($"Process name extracted: {processName}");
    }

    private void OnSessionCreated(string sessionPath)
    {
        string? processName = Path.GetFileNameWithoutExtension(sessionPath);
        Console.WriteLine($"Process name extracted: {processName}");
    }

    private void LoadMappings()
    {
        try
        {
            string json = File.ReadAllText(@"config/mappings.json");
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
            File.WriteAllText(@"config/mappings.json", jObj.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving mappings: {ex.Message}");
        }
    }


    public void OnSliderChanged(object? sender, SliderChangedEvent e)
    {
        Task.Run(() =>
        {
            if (!sliderMappings.TryGetValue(e.Group, out string? app))
            {
                return;
            }

            foreach (var session in defaultPlaybackDevice.SessionController.All())
            {
                if ((session.ExecutablePath != null && session.ExecutablePath.Contains(app, StringComparison.OrdinalIgnoreCase)) ||
        (session.DisplayName != null && session.DisplayName.Equals(app, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine($"Setting volume for {session.DisplayName} to {e.Value}%");
                    session.Volume = e.Value;
                }
            }
        });
    }

    public void OnDialChanged(object? sender, DialChangedEvent e)
    {
        Task.Run(() =>
        {
            if (!dialMappings.TryGetValue(e.Group, out string? app))
            {
                return;
            }

            foreach (var session in defaultPlaybackDevice.SessionController.All())
            {
                if ((session.ExecutablePath != null && session.ExecutablePath.Contains(app, StringComparison.OrdinalIgnoreCase)) ||
        (session.DisplayName != null && session.DisplayName.Equals(app, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine($"Setting volume for {session.DisplayName} to {e.Value}%");
                    session.Volume = e.Value;
                }
            }
        });
    }

    public void OnChangeApplicationOnSlider(object? sender, ChangeApplicationEvent e)
    {
        Task.Run(() =>
        {
            if (sliderMappings.TryGetValue(e.Group, out string? app))
            {
                sliderMappings.Remove(e.Group);
                LedStatusChanged?.Invoke(this, new LedChangedEvent(e.Button, false));
            }
            else
            {
                app = GetForegroundProcessName();
                if (app != null)
                {
                    sliderMappings.Add(e.Group, app);
                    LedStatusChanged?.Invoke(this, new LedChangedEvent(e.Button, true));
                }
            }
            SaveMappings();
        });
    }


    public static string? GetForegroundProcessName()
    {
        var hwnd = GetForegroundWindow();
        if (hwnd == IntPtr.Zero)
            return null;

        GetWindowThreadProcessId(hwnd, out uint pid);
        var proc = Process.GetProcessById((int)pid);
        return proc.ProcessName;
    }

}