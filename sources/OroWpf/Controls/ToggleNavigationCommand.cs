using System.Windows.Input;

namespace DustInTheWind.OroWpf.Controls;

public class ToggleNavigationCommand : ICommand
{
    private readonly PageEngine pageEngine;
    
    public event EventHandler CanExecuteChanged;
    
    public ToggleNavigationCommand(PageEngine pageEngine)
    {
        this.pageEngine = pageEngine ?? throw new ArgumentNullException(nameof(pageEngine));
    }
    
    public bool CanExecute(object parameter) => true;
    
    public void Execute(object parameter)
    {
        pageEngine.ToggleNavigation();
    }
}