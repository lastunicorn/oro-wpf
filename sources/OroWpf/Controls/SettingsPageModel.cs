using System.Reflection;
using DustInTheWind.OroWpf.Controls.About;
using DustInTheWind.OroWpf.Controls.Settings;
using DustInTheWind.OroWpf.Controls.Templates;

namespace DustInTheWind.OroWpf.Controls;

public class SettingsPageModel : PageViewModel
{
    public string Title { get; }

    public string Subtitle { get; }

    public TemplatesViewModel TemplatesViewModel { get; }

    public AboutViewModel AboutViewModel { get; }

    public SettingsViewModel SettingsViewModel { get; }

    public SettingsCloseCommand SettingsCloseCommand { get; }

    public SettingsPageModel(
        TemplatesViewModel templatesViewModel,
        SettingsViewModel settingsViewModel,
        AboutViewModel aboutViewModel,
        SettingsCloseCommand settingsCloseCommand)
    {
        TemplatesViewModel = templatesViewModel;
        AboutViewModel = aboutViewModel;
        SettingsViewModel = settingsViewModel;
        SettingsCloseCommand = settingsCloseCommand;

        Assembly assembly = Assembly.GetEntryAssembly();
        Title = assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;

        Version version = assembly.GetName().Version;
        Subtitle = version.ToString(3);
    }
}
