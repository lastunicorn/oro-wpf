using System.Windows;
using DustInTheWind.ClockWpf.Templates;
using DustInTheWind.OroWpf.Controls;
using DustInTheWind.OroWpf.Controls.About;
using DustInTheWind.OroWpf.Controls.Settings;
using DustInTheWind.OroWpf.Controls.Templates;
using DustInTheWind.OroWpf.Ports.SettingsAccess;
using Microsoft.Extensions.DependencyInjection;

namespace DustInTheWind.OroWpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        ServiceCollection serviceCollection = new();

        serviceCollection.AddSingleton<ISettings, Settings>();

        ApplicationState applicationState = CreateApplicationState();
        serviceCollection.AddSingleton(applicationState);

        PageEngine pageEngine = CreatePageEngine();
        serviceCollection.AddSingleton(pageEngine);
        serviceCollection.AddSingleton<IPageFactory, PageFactory>();

        serviceCollection.AddTransient<MainWindow>();
        serviceCollection.AddTransient<MainViewModel>();

        serviceCollection.AddTransient<ClockPage>();
        serviceCollection.AddTransient<ClockPageModel>();

        serviceCollection.AddTransient<SettingsPage>();
        serviceCollection.AddTransient<SettingsPageModel>();

        serviceCollection.AddTransient<TemplatesViewModel>();
        serviceCollection.AddTransient<SettingsViewModel>();
        serviceCollection.AddTransient<AboutViewModel>();
        serviceCollection.AddTransient<SettingsCloseCommand>();

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

