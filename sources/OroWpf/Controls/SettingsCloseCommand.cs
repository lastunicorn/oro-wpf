using System.Windows.Input;

namespace DustInTheWind.ClockWpf.ClearClock.Controls;

public class SettingsCloseCommand : ICommand
{
    private readonly PageEngine pageEngine;

    public event EventHandler CanExecuteChanged;
    
    public SettingsCloseCommand(PageEngine pageEngine)
    {
        this.pageEngine = pageEngine ?? throw new ArgumentNullException(nameof(pageEngine));
    }
    
    public bool CanExecute(object parameter) => true;
    
    public void Execute(object parameter)
    {
        pageEngine.CloseCurrentPage();
    }
}