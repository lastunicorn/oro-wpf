using System.Windows;
using DustInTheWind.ClockWpf.ClearClock.Controls;
using DustInTheWind.ClockWpf.Templates;
using Microsoft.Extensions.DependencyInjection;

namespace DustInTheWind.ClockWpf.ClearClock;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        ServiceCollection serviceCollection = new();

        ApplicationState applicationState = CreateApplicationState();
        serviceCollection.AddSingleton(applicationState);

        PageEngine pageEngine = CreatePageEngine();
        serviceCollection.AddSingleton(pageEngine);

        serviceCollection.AddTransient<MainWindow>();
        serviceCollection.AddTransient<MainViewModel>();

        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        MainWindow mainWindow = serviceProvider.GetService<MainWindow>();
        mainWindow.DataContext = serviceProvider.GetService<MainViewModel>();
        mainWindow.Show();

        MainWindow = mainWindow;

        base.OnStartup(e);
    }

    private static ApplicationState CreateApplicationState()
    {
        ApplicationState applicationState = new();

        List<Type> templateTypes = EnumerateClockTemplates().ToList();
        applicationState.AvailableTemplateTypes = templateTypes;

        if (templateTypes?.Count > 0)
        {
            Type selectedTemplateType = templateTypes
             .FirstOrDefault(x => x == typeof(DefaultTemplate));

            applicationState.ClockTemplate = (ClockTemplate)Activator.CreateInstance(selectedTemplateType);

            if (applicationState.ClockTemplate is DefaultTemplate defaultTemplate)
                defaultTemplate.Style = DefaultTemplate.TemplateStyle.Black;
        }

        return applicationState;
    }

    private static IEnumerable<Type> EnumerateClockTemplates()
    {
        return typeof(ClockTemplate).Assembly.GetTypes()
            .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(ClockTemplate)));
    }

    private static PageEngine CreatePageEngine()
    {
        PageEngine pageEngine = new();

        pageEngine.Pages.Add(new Page
        {
            Id = "clock",
            ViewType = typeof(ClockPage)
        });

        pageEngine.Pages.Add(new Page
        {
            Id = "settings",
            ViewType = typeof(SettingsPage)
        });

        pageEngine.SelectPage("clock");

        return pageEngine;
    }
}

