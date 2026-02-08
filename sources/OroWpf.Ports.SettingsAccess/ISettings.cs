namespace DustInTheWind.OroWpf.Ports.SettingsAccess;

public interface ISettings
{
    public bool KeepOnTop { get; set; }
    
    public bool Counterclockwise { get; set; }

    public double WindowLeft { get; set; }

    public double WindowTop { get; set; }

    public double WindowWidth { get; set; }

    public double WindowHeight { get; set; }

    public string ClockTemplateType { get; set; }

    public event EventHandler KeepOnTopChanged;
    event EventHandler CounterclockwiseChanged;

    void SetWindowLocation(double left, double top);

    void SetWindowSize(double width, double height);
}
