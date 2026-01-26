using DustInTheWind.OroWpf.Controls.About;
using DustInTheWind.OroWpf.Controls.Templates;

namespace DustInTheWind.OroWpf.Controls;

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
