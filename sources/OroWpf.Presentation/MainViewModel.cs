using System.Windows.Controls;
using DustInTheWind.OroWpf.Ports.SettingsAccess;
using DustInTheWind.OroWpf.Presentation.Controls;

namespace DustInTheWind.OroWpf.Presentation;

public class MainViewModel : ViewModelBase
{
    private readonly PageEngine pageEngine;
    private readonly ISettings settings;
    private readonly IPageFactory pageFactory;

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

    public bool KeepOnTop
    {
        get => field;
        set
        {
            if (field == value)
                return;

            field = value;
            OnPropertyChanged();

            if (!IsInitializing)
                settings.KeepOnTop = value;
        }
    }

    public SettingsCommand SettingsCommand { get; }

    public MainViewModel(PageEngine pageEngine, ISettings settings, IPageFactory pageFactory)
    {
        this.pageEngine = pageEngine ?? throw new ArgumentNullException(nameof(pageEngine));
        this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        this.pageFactory = pageFactory ?? throw new ArgumentNullException(nameof(pageFactory));
        SettingsCommand = new SettingsCommand(pageEngine);

        pageEngine.CurrentPageChanged += HandlePageChanged;
        pageEngine.IsNavigationVisibleChanged += HandleIsNavigationVisibleChanged;

        DisplayCurrentPage();

        Initialize(() =>
        {
            IsNavigationVisible = pageEngine.IsNavigationVisible;
            KeepOnTop = settings.KeepOnTop;
        });

        settings.KeepOnTopChanged += HandleKeepOnTopChanged;
    }

    private void HandleKeepOnTopChanged(object sender, EventArgs e)
    {
        KeepOnTop = settings.KeepOnTop;
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
            CurrentPage = pageFactory.CreatePage<SettingsPage, SettingsPageModel>();
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
        clockPage ??= pageFactory.CreatePage<ClockPage, ClockPageModel>();
        return clockPage;
    }

    private void HandleIsNavigationVisibleChanged(object sender, EventArgs e)
    {
        IsNavigationVisible = pageEngine.IsNavigationVisible;
    }
}
