using System.Windows.Controls;
using DustInTheWind.ClockWpf.ClearClock.Controls;

namespace DustInTheWind.ClockWpf.ClearClock;

public class MainViewModel : ViewModelBase
{
    private readonly ApplicationState applicationState;
    private PageEngine pageEngine;

    public Control CurrentPage
    {
        get => field;
        private set
        {
            if (field == value)
                return;

            field = value;

            OnPropertyChanged();
        }
    }

    public bool IsNavigationVisible
    {
        get => field;
        private set
        {
            if (field == value)
                return;

            field = value;
            OnPropertyChanged();
        }
    }

    public SettingsCommand SettingsCommand { get; }

    public MainViewModel(ApplicationState applicationState, PageEngine pageEngine)
    {
        ArgumentNullException.ThrowIfNull(applicationState);
        ArgumentNullException.ThrowIfNull(pageEngine);

        SettingsCommand = new SettingsCommand(pageEngine);

        this.applicationState = applicationState;
        this.pageEngine = pageEngine;

        pageEngine.CurrentPageChanged += HandlePageChanged;
        pageEngine.IsNavigationVisibleChanged += HandleIsNavigationVisibleChanged;

        DisplayCurrentPage();
    }

    private void HandlePageChanged(object sender, EventArgs e)
    {
        RemoveCurrentPage();
        DisplayCurrentPage();
    }

    private void RemoveCurrentPage()
    {
        if (CurrentPage != null)
        {
            if (CurrentPage.DataContext is PageViewModel pageViewModel)
                pageViewModel.PrepareForClose();

            CurrentPage = null;
        }
    }

    private void DisplayCurrentPage()
    {
        if (pageEngine.CurrentPage?.ViewType == typeof(SettingsPage))
        {
            SettingsPage settingsPage = new()
            {
                DataContext = new SettingsPageModel(applicationState, pageEngine)
            };
            CurrentPage = settingsPage;
        }
        else if (pageEngine.CurrentPage?.ViewType == typeof(ClockPage))
        {
            CurrentPage = GetOrCreateClockPage();
        }
        else
        {
            pageEngine.SelectPage("clock");
        }
    }

    private ClockPage clockPage;

    private ClockPage GetOrCreateClockPage()
    {
        if (clockPage == null)
        {
            clockPage = new ClockPage()
            {
                DataContext = new ClockPageModel(applicationState, pageEngine)
            };
        }

        return clockPage;
    }

    private void HandleIsNavigationVisibleChanged(object sender, EventArgs e)
    {
        IsNavigationVisible = pageEngine.IsNavigationVisible;
    }
}
