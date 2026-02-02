namespace DustInTheWind.OroWpf.Ports.SettingsAccess;

public interface ISettings
{
    public bool KeepOnTop { get; set; }

    public double WindowLeft { get; set; }

    public double WindowTop { get; set; }

    public event EventHandler KeepOnTopChanged;

    void SetWindowLocation(double left, double top);
}
