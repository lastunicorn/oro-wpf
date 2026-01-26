using System.Reflection;

namespace DustInTheWind.ClockWpf.ClearClock.Controls.About;

public class AboutViewModel : ViewModelBase
{
    private string version;

    public string Version
    {
        get => version;
        set
        {
            if (version == value)
                return;

            version = value;
            OnPropertyChanged();
        }
    }

    public AboutViewModel()
    {
        SetVersionInfo();
    }

    private void SetVersionInfo()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        Version version = assembly.GetName().Version;
        Version = version.ToString(3);
    }
}
