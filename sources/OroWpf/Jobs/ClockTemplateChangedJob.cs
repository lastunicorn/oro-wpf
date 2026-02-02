using DustInTheWind.OroWpf.Infrastructure.Jobs;
using DustInTheWind.OroWpf.Ports.SettingsAccess;
using DustInTheWind.OroWpf.Presentation;

namespace DustInTheWind.OroWpf.Jobs;

internal class ClockTemplateChangedJob : IJob
{
    private readonly ApplicationState applicationState;
    private readonly ISettings settings;

    public ClockTemplateChangedJob(ApplicationState applicationState, ISettings settings)
    {
        this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
        this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    public void Start()
    {
        applicationState.ClockTemplateChanged += HandleClockTemplateChanged;
    }

    public void Stop()
    {
        applicationState.ClockTemplateChanged -= HandleClockTemplateChanged;
    }

    private void HandleClockTemplateChanged(object sender, EventArgs e)
    {
        if (applicationState.ClockTemplate != null)
            settings.ClockTemplateType = applicationState.ClockTemplate.GetType().FullName;
    }
}
