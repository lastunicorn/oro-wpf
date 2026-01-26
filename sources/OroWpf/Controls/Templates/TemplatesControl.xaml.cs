using System.Windows.Controls;
using DustInTheWind.ClockWpf.TimeProviders;

namespace DustInTheWind.ClockWpf.ClearClock.Controls.Templates;

/// <summary>
/// Interaction logic for SettingsControl.xaml
/// </summary>
public partial class TemplatesControl : UserControl
{
    public TemplatesControl()
    {
        InitializeComponent();

        LocalTimeProvider localTimeProvider = new();
        localTimeProvider.Start();

        analogClock1.TimeProvider = localTimeProvider;
    }
}
