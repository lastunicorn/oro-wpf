using System.Windows.Controls;
using System.Windows.Input;
using DustInTheWind.OroWpf.Controls;

namespace DustInTheWind.OroWpf.Controls;

/// <summary>
/// Interaction logic for ClockPage.xaml
/// </summary>
public partial class ClockPage : UserControl
{
    public ClockPage()
    {
        InitializeComponent();
    }

    private void UserControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        if(DataContext is ClockPageModel viewModel)
            viewModel.ToggleNavigationCommand.Execute(null);
    }
}
