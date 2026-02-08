using DustInTheWind.ClockWpf.Shapes;
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

    public bool Counterclockwise
    {
        get => field;
        set
        {
            if (field == value)
                return;

            field = value;
            OnPropertyChanged();

            if (!IsInitializing)
                settings.Counterclockwise = value;
        }
    }

    public RotationDirection ClockDirection
    {
        get => field;
        set
        {
            if (field == value)
                return;

            field = value;
            OnPropertyChanged();

            if (!IsInitializing)
            {
                settings.Counterclockwise = value switch
                {
                    RotationDirection.Clockwise => false,
                    RotationDirection.Counterclockwise => true,
                    _ => throw new InvalidOperationException($"Unknown rotation direction: {value}")
                };
            }
        }
    }

    public SettingsViewModel(ISettings settings)
    {
        this.settings = settings ?? throw new ArgumentNullException(nameof(settings));

        settings.CounterclockwiseChanged += HandleCounterclockwiseChanged; ;

        Initialize();
    }

    private void HandleCounterclockwiseChanged(object sender, EventArgs e)
    {
        Counterclockwise = settings.Counterclockwise;
        ClockDirection = settings.Counterclockwise
            ? RotationDirection.Counterclockwise
            : RotationDirection.Clockwise;
    }

    private void Initialize()
    {
        Initialize(() =>
        {
            KeepOnTop = settings.KeepOnTop;
            Counterclockwise = settings.Counterclockwise;
            ClockDirection = settings.Counterclockwise
                ? RotationDirection.Counterclockwise
                : RotationDirection.Clockwise;
        });
    }
}
