using DustInTheWind.ClockWpf.Templates;
using DustInTheWind.ClockWpf.TimeProviders;

namespace DustInTheWind.ClockWpf.ClearClock.Controls;

public class ClockPageModel : PageViewModel
{
    private readonly ApplicationState applicationState;

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

    public ITimeProvider TimeProvider
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

    public ClockPageModel(ApplicationState applicationState, PageEngine pageEngine)
    {
        ArgumentNullException.ThrowIfNull(pageEngine);
        this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));

        ToggleNavigationCommand = new ToggleNavigationCommand(pageEngine);

        applicationState.ClockTemplateChanged += HandleClockTemplateChanged;
        ClockTemplate = applicationState.ClockTemplate;

        TimeProvider = new LocalTimeProvider();
        TimeProvider.Start();
        this.applicationState = applicationState;
    }

    private void HandleClockTemplateChanged(object sender, EventArgs e)
    {
        ClockTemplate = applicationState.ClockTemplate;
    }
}
