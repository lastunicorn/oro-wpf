using DustInTheWind.ClockWpf.Movements;
using DustInTheWind.ClockWpf.Shapes;
using DustInTheWind.ClockWpf.Templates;
using DustInTheWind.OroWpf.Infrastructure.PageModel;
using DustInTheWind.OroWpf.Ports.SettingsAccess;

namespace DustInTheWind.OroWpf.Presentation.Controls;

public class ClockPageModel : PageViewModel
{
    private readonly ApplicationState applicationState;
    private readonly ISettings settings;

    public ClockTemplate ClockTemplate
    {
        get => field;
        set
        {
            if (field == value)
                return;

            field = value;
            OnPropertyChanged();
        }
    }

    public IMovement ClockMovement
    {
        get => field;
        set
        {
            if (field == value)
                return;

            field = value;

            OnPropertyChanged();
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
        }
    }

    public ToggleNavigationCommand ToggleNavigationCommand { get; }

    public ClockPageModel(ApplicationState applicationState, PageEngine pageEngine, ISettings settings)
    {
        ArgumentNullException.ThrowIfNull(pageEngine);
        this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
        this.settings = settings ?? throw new ArgumentNullException(nameof(settings));

        ToggleNavigationCommand = new ToggleNavigationCommand(pageEngine);

        applicationState.ClockTemplateChanged += HandleClockTemplateChanged;
        settings.CounterclockwiseChanged += HandleCounterclockwiseChanged;
        settings.RefreshRateChanged += HandleRefreshRateChanged;

        Initialize();
    }

    private void Initialize()
    {
        Initialize(() =>
        {
            ClockTemplate = applicationState.ClockTemplate;

            LocalTimeMovement clockMovement = new()
            {
                TickInterval = (int)Math.Round(TimeSpan.MillisecondsPerSecond / settings.RefreshRate)
            };
            clockMovement.Start();

            ClockMovement = clockMovement;

            ClockDirection = settings.Counterclockwise
                ? RotationDirection.Counterclockwise
                : RotationDirection.Clockwise;
        });
    }

    private void HandleClockTemplateChanged(object sender, EventArgs e)
    {
        Initialize(() =>
        {
            ClockTemplate = applicationState.ClockTemplate;
        });
    }

    private void HandleCounterclockwiseChanged(object sender, EventArgs e)
    {
        Initialize(() =>
        {
            ClockDirection = settings.Counterclockwise
                ? RotationDirection.Counterclockwise
                : RotationDirection.Clockwise;
        });
    }

    private void HandleRefreshRateChanged(object sender, EventArgs e)
    {
        if (ClockMovement == null)
            return;

        Initialize(() =>
        {
            ClockMovement.TickInterval = (int)Math.Round(TimeSpan.MillisecondsPerSecond / settings.RefreshRate);
        });
    }
}
