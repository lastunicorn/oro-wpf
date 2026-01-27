using DustInTheWind.OroWpf.Ports.SettingsAccess;

namespace DustInTheWind.OroWpf.Presentation.Controls.Settings;

public class SettingsViewModel : ViewModelBase
{
    private readonly ISettings settings;

    public bool KeepOnTop
    {
        get => field;
        set
        {
            if (field == value)
                return;

            field = value;
            OnPropertyChanged();

            if (!IsInitializing)
                settings.KeepOnTop = value;
        }
    }

    public SettingsViewModel(ISettings settings)
    {
        this.settings = settings ?? throw new ArgumentNullException(nameof(settings));

        Initialize(() =>
        {
            KeepOnTop = settings.KeepOnTop;
        });
    }
}
