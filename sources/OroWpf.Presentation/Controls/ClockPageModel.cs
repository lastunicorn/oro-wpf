using DustInTheWind.ClockWpf.Movements;
using DustInTheWind.ClockWpf.Templates;
using DustInTheWind.OroWpf.Presentation.CustomControls.PageModel;

namespace DustInTheWind.OroWpf.Presentation.Controls;

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

    public ToggleNavigationCommand ToggleNavigationCommand { get; }

    public ClockPageModel(ApplicationState applicationState, PageEngine pageEngine)
    {
        ArgumentNullException.ThrowIfNull(pageEngine);
        this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));

        ToggleNavigationCommand = new ToggleNavigationCommand(pageEngine);

        applicationState.ClockTemplateChanged += HandleClockTemplateChanged;
        ClockTemplate = applicationState.ClockTemplate;

        ClockMovement = new LocalTimeMovement();
        ClockMovement.Start();

        this.applicationState = applicationState;
    }

    private void HandleClockTemplateChanged(object sender, EventArgs e)
    {
        ClockTemplate = applicationState.ClockTemplate;
    }
}
