namespace DustInTheWind.OroWpf.Ports.SettingsAccess;

public interface ISettings
{
    public bool KeepOnTop { get; set; }

    public event EventHandler KeepOnTopChanged;
}
