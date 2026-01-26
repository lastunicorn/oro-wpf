using DustInTheWind.ClockWpf.ClearClock.Controls.About;
using DustInTheWind.ClockWpf.ClearClock.Controls.Templates;

namespace DustInTheWind.ClockWpf.ClearClock.Controls;

public class SettingsPageModel : PageViewModel
{
    public TemplatesViewModel TemplatesViewModel { get; }

    public AboutViewModel AboutViewModel { get; }

    public SettingsCloseCommand SettingsCloseCommand { get; }

    public SettingsPageModel(ApplicationState applicationState, PageEngine pageEngine)
    {
        ArgumentNullException.ThrowIfNull(applicationState);

        TemplatesViewModel = new TemplatesViewModel(applicationState);
        AboutViewModel = new AboutViewModel();
        SettingsCloseCommand = new SettingsCloseCommand(pageEngine);
    }
}
