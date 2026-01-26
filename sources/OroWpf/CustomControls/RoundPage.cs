using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DustInTheWind.ClockWpf.ClearClock.CustomControls;

/// <summary>
/// Interaction logic for RoundPage.xaml
/// </summary>
public class RoundPage : UserControl
{
    #region Title Dependency Property

    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        nameof(Title),
        typeof(string),
        typeof(RoundPage),
        new PropertyMetadata(string.Empty));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    #endregion

    #region CloseCommand Dependency Property

    public static readonly DependencyProperty CloseCommandProperty = DependencyProperty.Register(
        nameof(CloseCommand),
        typeof(ICommand),
        typeof(RoundPage),
        new PropertyMetadata(default(ICommand)));

    public ICommand CloseCommand
    {
        get => (ICommand)GetValue(CloseCommandProperty);
        set => SetValue(CloseCommandProperty, value);
    }

    #endregion

    static RoundPage()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(RoundPage), new FrameworkPropertyMetadata(typeof(RoundPage)));
    }
}
