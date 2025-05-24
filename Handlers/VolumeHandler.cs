
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NanoKontrolVolume;

public class VolumeHandler
{
    Dictionary<int, string> sliderMappings = new Dictionary<int, string>();
    Dictionary<int, string> dialMappings = new Dictionary<int, string>();

    public VolumeHandler()
    {
        LoadMappings();
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


}