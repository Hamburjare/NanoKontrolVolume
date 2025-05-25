namespace NanoKontrolVolume;

public class ButtonHandler
{
    public void OnButtonPress(object? sender, ButtonPressEvent e)
    {
        Task.Run(() =>
        {
            Console.WriteLine($"Slider changed: Group {e.Group}, IsPressed {e.IsPressed}, Button {e.Button}");
        });
    }
}