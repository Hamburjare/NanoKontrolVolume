using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;

namespace NanoKontrolVolume;

public class ButtonHandler
{
    public event EventHandler<LedChangedEvent>? LedStatusChanged;
    public event EventHandler<ChangeApplicationEvent>? ChangeApplicationOnSlider;
    public event EventHandler<MuteApplicationEvent>? MuteApplicationOnSlider;

    public enum VKeys
    {
        /// <summary>
        /// Left mouse button.
        /// </summary>
        LBUTTON = 0x01,
        /// <summary>
        /// Right mouse button.
        /// </summary>
        RBUTTON = 0x02,
        /// <summary>
        /// Control-break processing.
        /// </summary>
        CANCEL = 0x03,
        /// <summary>
        /// Middle mouse button (three-button mouse).
        /// </summary>
        MBUTTON = 0x04,
        /// <summary>
        /// Windows 2000/XP: X1 mouse button.
        /// </summary>
        XBUTTON1 = 0x05,
        /// <summary>
        /// Windows 2000/XP: X2 mouse button.
        /// </summary>
        XBUTTON2 = 0x06,
        // 0x07 - Undefined.
        /// <summary>
        /// BACKSPACE key.
        /// </summary>
        BACK = 0x08,
        /// <summary>
        /// TAB key.
        /// </summary>
        TAB = 0x09,
        // 0x0A-0x0B - Reserved.
        /// <summary>
        /// CLEAR key.
        /// </summary>
        CLEAR = 0x0C,
        /// <summary>
        /// ENTER key.
        /// </summary>
        RETURN = 0x0D,
        // 0x0E-0x0F - Undefined.
        /// <summary>
        /// SHIFT key.
        /// </summary>
        SHIFT = 0x10,
        /// <summary>
        /// CTRL key.
        /// </summary>
        CONTROL = 0x11,
        /// <summary>
        /// ALT key.
        /// </summary>
        MENU = 0x12,
        /// <summary>
        /// PAUSE key.
        /// </summary>
        PAUSE = 0x13,
        /// <summary>
        /// CAPS LOCK key.
        /// </summary>
        CAPITAL = 0x14,
        /// <summary>
        /// Input Method Editor (IME) Kana mode.
        /// </summary>
        KANA = 0x15,
        /// <summary>
        /// IME Hangul mode.
        /// </summary>
        HANGUL = 0x15,
        // 0x16 - Undefined
        /// <summary>
        /// IME Junja mode.
        /// </summary>
        JUNJA = 0x17,
        /// <summary>
        /// IME final mode.
        /// </summary>
        FINAL = 0x18,
        /// <summary>
        /// IME Hanja mode.
        /// </summary>
        HANJA = 0x19,
        /// <summary>
        /// IME Kanji mode.
        /// </summary>
        KANJI = 0x19,
        // 0x1A - Undefined.
        /// <summary>
        /// ESC key.
        /// </summary>
        ESCAPE = 0x1B,
        /// <summary>
        /// IME convert.
        /// </summary>
        CONVERT = 0x1C,
        /// <summary>
        /// IME nonconvert.
        /// </summary>
        NONCONVERT = 0x1D,
        /// <summary>
        /// IME accept.
        /// </summary>
        ACCEPT = 0x1E,
        /// <summary>
        /// IME mode change request.
        /// </summary>
        MODECHANGE = 0x1F,
        /// <summary>
        /// SPACEBAR.
        /// </summary>
        SPACE = 0x20,
        /// <summary>
        /// PAGE UP key.
        /// </summary>
        PRIOR = 0x21,
        /// <summary>
        /// PAGE DOWN key.
        /// </summary>
        NEXT = 0x22,
        /// <summary>
        /// END key.
        /// </summary>
        END = 0x23,
        /// <summary>
        /// HOME key.
        /// </summary>
        HOME = 0x24,
        /// <summary>
        /// LEFT ARROW key.
        /// </summary>
        LEFT = 0x25,
        /// <summary>
        /// UP ARROW key.
        /// </summary>
        UP = 0x26,
        /// <summary>
        /// RIGHT ARROW key.
        /// </summary>
        RIGHT = 0x27,
        /// <summary>
        /// DOWN ARROW key.
        /// </summary>
        DOWN = 0x28,
        /// <summary>
        /// SELECT key.
        /// </summary>
        SELECT = 0x29,
        /// <summary>
        /// PRINT key.
        /// </summary>
        PRINT = 0x2A,
        /// <summary>
        /// EXECUTE key.
        /// </summary>
        EXECUTE = 0x2B,
        /// <summary>
        /// PRINT SCREEN key.
        /// </summary>
        SNAPSHOT = 0x2C,
        /// <summary>
        /// INS key.
        /// </summary>
        INSERT = 0x2D,
        /// <summary>
        /// DEL key.
        /// </summary>
        DELETE = 0x2E,
        /// <summary>
        /// HELP key.
        /// </summary>
        HELP = 0x2F,
        /// <summary>
        /// 0 key.
        /// </summary>
        KEY_0 = 0x30,
        /// <summary>
        /// 1 key.
        /// </summary>
        KEY_1 = 0x31,
        /// <summary>
        /// 2 key.
        /// </summary>
        KEY_2 = 0x32,
        /// <summary>
        /// 3 key.
        /// </summary>
        KEY_3 = 0x33,
        /// <summary>
        /// 4 key.
        /// </summary>
        KEY_4 = 0x34,
        /// <summary>
        /// 5 key.
        /// </summary>
        KEY_5 = 0x35,
        /// <summary>
        /// 6 key.
        /// </summary>
        KEY_6 = 0x36,
        /// <summary>
        /// 7 key.
        /// </summary>
        KEY_7 = 0x37,
        /// <summary>
        /// 8 key.
        /// </summary>
        KEY_8 = 0x38,
        /// <summary>
        /// 9 key.
        /// </summary>
        KEY_9 = 0x39,
        // 0x3A-0x40 - Undefined.
        /// <summary>
        /// A key.
        /// </summary>
        KEY_A = 0x41,
        /// <summary>
        /// B key.
        /// </summary>
        KEY_B = 0x42,
        /// <summary>
        /// C key.
        /// </summary>
        KEY_C = 0x43,
        /// <summary>
        /// D key.
        /// </summary>
        KEY_D = 0x44,
        /// <summary>
        /// E key.
        /// </summary>
        KEY_E = 0x45,
        /// <summary>
        /// F key.
        /// </summary>
        KEY_F = 0x46,
        /// <summary>
        /// G key.
        /// </summary>
        KEY_G = 0x47,
        /// <summary>
        /// H key.
        /// </summary>
        KEY_H = 0x48,
        /// <summary>
        /// I key.
        /// </summary>
        KEY_I = 0x49,
        /// <summary>
        /// J key.
        /// </summary>
        KEY_J = 0x4A,
        /// <summary>
        /// K key.
        /// </summary>
        KEY_K = 0x4B,
        /// <summary>
        /// L key.
        /// </summary>
        KEY_L = 0x4C,
        /// <summary>
        /// M key.
        /// </summary>
        KEY_M = 0x4D,
        /// <summary>
        /// N key.
        /// </summary>
        KEY_N = 0x4E,
        /// <summary>
        /// O key.
        /// </summary>
        KEY_O = 0x4F,
        /// <summary>
        /// P key.
        /// </summary>
        KEY_P = 0x50,
        /// <summary>
        /// Q key.
        /// </summary>
        KEY_Q = 0x51,
        /// <summary>
        /// R key.
        /// </summary>
        KEY_R = 0x52,
        /// <summary>
        /// S key.
        /// </summary>
        KEY_S = 0x53,
        /// <summary>
        /// T key.
        /// </summary>
        KEY_T = 0x54,
        /// <summary>
        /// U key.
        /// </summary>
        KEY_U = 0x55,
        /// <summary>
        /// V key.
        /// </summary>
        KEY_V = 0x56,
        /// <summary>
        /// W key.
        /// </summary>
        KEY_W = 0x57,
        /// <summary>
        /// X key.
        /// </summary>
        KEY_X = 0x58,
        /// <summary>
        /// Y key.
        /// </summary>
        KEY_Y = 0x59,
        /// <summary>
        /// Z key.
        /// </summary>
        KEY_Z = 0x5A,
        /// <summary>
        /// Left Windows key (Microsoft Natural keyboard).
        /// </summary>
        LWIN = 0x5B,
        /// <summary>
        /// Right Windows key (Natural keyboard).
        /// </summary>
        RWIN = 0x5C,
        /// <summary>
        /// Applications key (Natural keyboard).
        /// </summary>
        APPS = 0x5D,
        // 0x5E - Reserved.
        /// <summary>
        /// Computer Sleep key.
        /// </summary>
        SLEEP = 0x5F,
        /// <summary>
        /// Numeric keypad 0 key.
        /// </summary>
        NUMPAD0 = 0x60,
        /// <summary>
        /// Numeric keypad 1 key.
        /// </summary>
        NUMPAD1 = 0x61,
        /// <summary>
        /// Numeric keypad 2 key.
        /// </summary>
        NUMPAD2 = 0x62,
        /// <summary>
        /// Numeric keypad 3 key.
        /// </summary>
        NUMPAD3 = 0x63,
        /// <summary>
        /// Numeric keypad 4 key.
        /// </summary>
        NUMPAD4 = 0x64,
        /// <summary>
        /// Numeric keypad 5 key.
        /// </summary>
        NUMPAD5 = 0x65,
        /// <summary>
        /// Numeric keypad 6 key.
        /// </summary>
        NUMPAD6 = 0x66,
        /// <summary>
        /// Numeric keypad 7 key.
        /// </summary>
        NUMPAD7 = 0x67,
        /// <summary>
        /// Numeric keypad 8 key.
        /// </summary>
        NUMPAD8 = 0x68,
        /// <summary>
        /// Numeric keypad 9 key.
        /// </summary>
        NUMPAD9 = 0x69,
        /// <summary>
        /// Multiply key.
        /// </summary>
        MULTIPLY = 0x6A,
        /// <summary>
        /// Add key.
        /// </summary>
        ADD = 0x6B,
        /// <summary>
        /// Separator key.
        /// </summary>
        SEPARATOR = 0x6C,
        /// <summary>
        /// Subtract key.
        /// </summary>
        SUBTRACT = 0x6D,
        /// <summary>
        /// Decimal key.
        /// </summary>
        DECIMAL = 0x6E,
        /// <summary>
        /// Divide key.
        /// </summary>
        DIVIDE = 0x6F,
        /// <summary>
        /// F1 key.
        /// </summary>
        F1 = 0x70,
        /// <summary>
        /// F2 key.
        /// </summary>
        F2 = 0x71,
        /// <summary>
        /// F3 key.
        /// </summary>
        F3 = 0x72,
        /// <summary>
        /// F4 key.
        /// </summary>
        F4 = 0x73,
        /// <summary>
        /// F5 key.
        /// </summary>
        F5 = 0x74,
        /// <summary>
        /// F6 key.
        /// </summary>
        F6 = 0x75,
        /// <summary>
        /// F7 key.
        /// </summary>
        F7 = 0x76,
        /// <summary>
        /// F8 key.
        /// </summary>
        F8 = 0x77,
        /// <summary>
        /// F9 key.
        /// </summary>
        F9 = 0x78,
        /// <summary>
        /// F10 key.
        /// </summary>
        F10 = 0x79,
        /// <summary>
        /// F11 key.
        /// </summary>
        F11 = 0x7A,
        /// <summary>
        /// F12 key.
        /// </summary>
        F12 = 0x7B,
        /// <summary>
        /// F13 key.
        /// </summary>
        F13 = 0x7C,
        /// <summary>
        /// F14 key.
        /// </summary>
        F14 = 0x7D,
        /// <summary>
        /// F15 key.
        /// </summary>
        F15 = 0x7E,
        /// <summary>
        /// F16 key.
        /// </summary>
        F16 = 0x7F,
        /// <summary>
        /// F17 key.
        /// </summary>
        F17 = 0x80,
        /// <summary>
        /// F18 key.
        /// </summary>
        F18 = 0x81,
        /// <summary>
        /// F19 key.
        /// </summary>
        F19 = 0x82,
        /// <summary>
        /// F20 key.
        /// </summary>
        F20 = 0x83,
        /// <summary>
        /// F21 key.
        /// </summary>
        F21 = 0x84,
        /// <summary>
        /// F22 key, (PPC only) Key used to lock device.
        /// </summary>
        F22 = 0x85,
        /// <summary>
        /// F23 key.
        /// </summary>
        F23 = 0x86,
        /// <summary>
        /// F24 key.
        /// </summary>
        F24 = 0x87,
        // 0x88-0X8F - Unassigned.
        /// <summary>
        /// NUM LOCK key.
        /// </summary>
        NUMLOCK = 0x90,
        /// <summary>
        /// SCROLL LOCK key.
        /// </summary>
        SCROLL = 0x91,
        // 0x92-0x96 - OEM specific.
        // 0x97-0x9F - Unassigned.
        /// <summary>
        /// Left SHIFT key.
        /// </summary>
        LSHIFT = 0xA0,
        /// <summary>
        /// Right SHIFT key.
        /// </summary>
        RSHIFT = 0xA1,
        /// <summary>
        /// Left CONTROL key.
        /// </summary>
        LCONTROL = 0xA2,
        /// <summary>
        /// Right CONTROL key.
        /// </summary>
        RCONTROL = 0xA3,
        /// <summary>
        /// Left MENU key.
        /// </summary>
        LMENU = 0xA4,
        /// <summary>
        /// Right MENU key.
        /// </summary>
        RMENU = 0xA5,
        /// <summary>
        /// Windows 2000/XP: Browser Back key.
        /// </summary>
        BROWSER_BACK = 0xA6,
        /// <summary>
        /// Windows 2000/XP: Browser Forward key.
        /// </summary>
        BROWSER_FORWARD = 0xA7,
        /// <summary>
        /// Windows 2000/XP: Browser Refresh key.
        /// </summary>
        BROWSER_REFRESH = 0xA8,
        /// <summary>
        /// Windows 2000/XP: Browser Stop key.
        /// </summary>
        BROWSER_STOP = 0xA9,
        /// <summary>
        /// Windows 2000/XP: Browser Search key.
        /// </summary>
        BROWSER_SEARCH = 0xAA,
        /// <summary>
        /// Windows 2000/XP: Browser Favorites key.
        /// </summary>
        BROWSER_FAVORITES = 0xAB,
        /// <summary>
        /// Windows 2000/XP: Browser Start and Home key.
        /// </summary>
        BROWSER_HOME = 0xAC,
        /// <summary>
        /// Windows 2000/XP: Volume Mute key.
        /// </summary>
        VOLUME_MUTE = 0xAD,
        /// <summary>
        /// Windows 2000/XP: Volume Down key.
        /// </summary>
        VOLUME_DOWN = 0xAE,
        /// <summary>
        /// Windows 2000/XP: Volume Up key.
        /// </summary>
        VOLUME_UP = 0xAF,
        /// <summary>
        /// Windows 2000/XP: Next Track key.
        /// </summary>
        MEDIA_NEXT_TRACK = 0xB0,
        /// <summary>
        /// Windows 2000/XP: Previous Track key.
        /// </summary>
        MEDIA_PREV_TRACK = 0xB1,
        /// <summary>
        /// Windows 2000/XP: Stop Media key.
        /// </summary>
        MEDIA_STOP = 0xB2,
        /// <summary>
        /// Windows 2000/XP: Play/Pause Media key.
        /// </summary>
        MEDIA_PLAY_PAUSE = 0xB3,
        /// <summary>
        /// Windows 2000/XP: Start Mail key.
        /// </summary>
        LAUNCH_MAIL = 0xB4,
        /// <summary>
        /// Windows 2000/XP: Select Media key.
        /// </summary>
        LAUNCH_MEDIA_SELECT = 0xB5,
        /// <summary>
        /// Windows 2000/XP: Start Application 1 key.
        /// </summary>
        LAUNCH_APP1 = 0xB6,
        /// <summary>
        /// Windows 2000/XP: Start Application 2 key.
        /// </summary>
        LAUNCH_APP2 = 0xB7,
        // 0xB8-0xB9 - Reserved.
        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard.
        /// Windows 2000/XP: For the US standard keyboard, the ';:' key
        /// </summary>
        OEM_1 = 0xBA,
        /// <summary>
        /// Windows 2000/XP: For any country/region, the '+' key.
        /// </summary>
        OEM_PLUS = 0xBB,
        /// <summary>
        /// Windows 2000/XP: For any country/region, the ',' key.
        /// </summary>
        OEM_COMMA = 0xBC,
        /// <summary>
        /// Windows 2000/XP: For any country/region, the '-' key.
        /// </summary>
        OEM_MINUS = 0xBD,
        /// <summary>
        /// Windows 2000/XP: For any country/region, the '.' key.
        /// </summary>
        OEM_PERIOD = 0xBE,
        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard.
        /// Windows 2000/XP: For the US standard keyboard, the '/?' key.
        /// </summary>
        OEM_2 = 0xBF,
        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard.
        /// Windows 2000/XP: For the US standard keyboard, the '`~' key.
        /// </summary>
        OEM_3 = 0xC0,
        // 0xC1-0xD7 - Reserved.
        // 0xD8-0xDA - Unassigned.
        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard.
        /// Windows 2000/XP: For the US standard keyboard, the '[{' key.
        /// </summary>
        OEM_4 = 0xDB,
        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard.
        /// Windows 2000/XP: For the US standard keyboard, the '\|' key.
        /// </summary>
        OEM_5 = 0xDC,
        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard.
        /// Windows 2000/XP: For the US standard keyboard, the ']}' key.
        /// </summary>
        OEM_6 = 0xDD,
        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard.
        /// Windows 2000/XP: For the US standard keyboard, the 'single-quote/double-quote' key.
        /// </summary>
        OEM_7 = 0xDE,
        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard.
        /// </summary>
        OEM_8 = 0xDF,
        // 0xE0 -  Reserved.
        // 0xE1 - OEM specific.
        /// <summary>
        /// Windows 2000/XP: Either the angle bracket key or the backslash key on the RT 102-key keyboard.
        /// </summary>
        OEM_102 = 0xE2,
        // 0xE3-E4 - OEM specific.
        /// <summary>
        /// Windows 95/98/Me, Windows NT 4.0, Windows 2000/XP: IME PROCESS key.
        /// </summary>
        PROCESSKEY = 0xE5,
        // 0xE6 - OEM specific.
        /// <summary>
        /// Windows 2000/XP: Used to pass Unicode characters as if they were keystrokes. 
        /// The VK_PACKET key is the low word of a 32-bit Virtual Key value used for non-keyboard input methods. For more information, 
        /// see Remark in <see href="https://msdn.microsoft.com/ru-ru/library/windows/desktop/ms646271(v=vs.85).aspx">KEYBDINPUT</see>, 
        /// <see href="https://msdn.microsoft.com/ru-ru/library/windows/desktop/ms646310(v=vs.85).aspx">SendInput</see>, 
        /// <see href="https://msdn.microsoft.com/ru-ru/library/windows/desktop/ms646280(v=vs.85).aspx">WM_KEYDOWN</see> and 
        /// <see href="https://msdn.microsoft.com/ru-ru/library/windows/desktop/ms646281(v=vs.85).aspx">WM_KEYUP</see>.
        /// </summary>
        PACKET = 0xE7,
        // 0xE8 - Unassigned.
        // 0xE9-F5 - OEM specific.
        /// <summary>
        /// Attn key.
        /// </summary>
        ATTN = 0xF6,
        /// <summary>
        /// CrSel key.
        /// </summary>
        CRSEL = 0xF7,
        /// <summary>
        /// ExSel key.
        /// </summary>
        EXSEL = 0xF8,
        /// <summary>
        /// Erase EOF key.
        /// </summary>
        EREOF = 0xF9,
        /// <summary>
        /// Play key.
        /// </summary>
        PLAY = 0xFA,
        /// <summary>
        /// Zoom key.
        /// </summary>
        ZOOM = 0xFB,
        /// <summary>
        /// Reserved.
        /// </summary>
        NONAME = 0xFC,
        /// <summary>
        /// PA1 key.
        /// </summary>
        PA1 = 0xFD,
        /// <summary>
        /// Clear key.
        /// </summary>
        OEM_CLEAR = 0xFE
    }

    [DllImport("user32.dll", SetLastError = true)]
    public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

    public static void SendMediaKey(byte keyCode)
    {
        const int KEYEVENTF_KEYDOWN = 0x0000;
        const int KEYEVENTF_KEYUP = 0x0002;

        keybd_event(keyCode, 0, KEYEVENTF_KEYDOWN, UIntPtr.Zero);
        keybd_event(keyCode, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
    }

    Dictionary<string, string> buttonMappings = new Dictionary<string, string>();

    public ButtonHandler()
    {
        LoadMappings();
    }

    public void OnButtonPress(object? sender, ButtonPressEvent e)
    {

        if (IsSoloButton(e.Button) && e.IsPressed)
        {
            ChangeApplicationOnSlider?.Invoke(this, new ChangeApplicationEvent(e.Button, e.Group));
        }
        else if (IsMuteButton(e.Button) && e.IsPressed)
        {
            MuteApplicationOnSlider?.Invoke(this, new MuteApplicationEvent(e.Button, e.Group));
        }

        if (buttonMappings.TryGetValue(e.Button.ToString(), out string? action) && e.IsPressed)
        {

            if (action == "Media: Play/Pause")
            {
                SendMediaKey((byte)VKeys.MEDIA_PLAY_PAUSE);
            }
            else if (action == "Media: Stop")
            {
                SendMediaKey((byte)VKeys.MEDIA_STOP);
            }
            else if (action == "Media: Next")
            {
                SendMediaKey((byte)VKeys.MEDIA_NEXT_TRACK);
            }
            else if (action == "Media: Previous")
            {
                SendMediaKey((byte)VKeys.MEDIA_PREV_TRACK);
            }
            else if (action.StartsWith("Send Key Combination: "))
            {
                action = action.Substring("Send Key Combination: ".Length).Trim();
                SendKeyCombinationFromString(action);
            }
        }
    }

    public void LoadMappings()
    {
        try
        {
            string json = File.ReadAllText(@"config/buttonMappings.json");
            var jObj = JObject.Parse(json);
            buttonMappings.Clear();
            foreach (var pair in jObj)
            {
                buttonMappings[pair.Key] = pair.Value?.ToString() ?? "";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading button mappings: {ex.Message}");
        }
    }

    public void SaveMappings()
    {
        try
        {
            var jObj = new JObject();
            foreach (var pair in buttonMappings)
            {
                jObj[pair.Key] = pair.Value;
            }
            File.WriteAllText(@"config/buttonMappings.json", jObj.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving button mappings: {ex.Message}");
        }
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

    public static void SendKeyCombinationFromString(string combo)
    {
        // Map common names to enum values
        var keyMap = new Dictionary<string, VKeys>(StringComparer.OrdinalIgnoreCase)
    {
        { "Ctrl", VKeys.CONTROL },
        { "Control", VKeys.CONTROL },
        { "Alt", VKeys.MENU },
        { "Shift", VKeys.SHIFT },
        { "LShift", VKeys.LSHIFT},
        { "Win", VKeys.LWIN },
        { "Del", VKeys.DELETE },
        { "Delete", VKeys.DELETE },
        { "Esc", VKeys.ESCAPE },
        { "Escape", VKeys.ESCAPE },
        { "Tab", VKeys.TAB },
        { "Enter", VKeys.RETURN },
    };

        var parts = combo.Split(new[] { '+', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        var keys = new List<VKeys>();

        foreach (var part in parts)
        {
            if (keyMap.TryGetValue(part, out var vkey))
            {
                keys.Add(vkey);
            }
            else
            {
                // Try to parse as enum name, e.g. "F4"
                if (Enum.TryParse<VKeys>("KEY_" + part.ToUpper(), out vkey) ||
                    Enum.TryParse<VKeys>(part.ToUpper(), out vkey))
                {
                    keys.Add(vkey);
                }
            }
        }

        const int KEYEVENTF_KEYDOWN = 0x0000;
        const int KEYEVENTF_KEYUP = 0x0002;

        // Press all keys down
        foreach (var key in keys)
        {
            keybd_event((byte)key, 0, KEYEVENTF_KEYDOWN, UIntPtr.Zero);
        }
        // Release all keys up (in reverse order)
        for (int i = keys.Count - 1; i >= 0; i--)
        {
            keybd_event((byte)keys[i], 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
        }
    }

    public void TurnButtonOnLeds()
    {
        Shutdown();
        foreach (var button in buttonMappings)
        {
            if (string.IsNullOrEmpty(button.Value))
                continue;
            LedStatusChanged?.Invoke(this, new LedChangedEvent((MidiHandler.ControlID)Enum.Parse(typeof(MidiHandler.ControlID), $"{button.Key}"), true));
        }
    }

    public void Shutdown()
    {
        foreach (var button in buttonMappings.Keys)
        {
            LedStatusChanged?.Invoke(this, new LedChangedEvent((MidiHandler.ControlID)Enum.Parse(typeof(MidiHandler.ControlID), $"{button}"), false));
        }
    }
}