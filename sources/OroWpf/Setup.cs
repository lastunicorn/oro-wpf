using DustInTheWind.ClockWpf.Templates;
using DustInTheWind.OroWpf.Ports.SettingsAccess;
using DustInTheWind.OroWpf.Presentation;
using DustInTheWind.OroWpf.Presentation.Controls;
using DustInTheWind.OroWpf.Presentation.Controls.About;
using DustInTheWind.OroWpf.Presentation.Controls.Settings;
using DustInTheWind.OroWpf.Presentation.Controls.Templates;
using Microsoft.Extensions.DependencyInjection;

namespace DustInTheWind.OroWpf;

internal static class  Setup
{
    public static void ConfigureServices(IServiceCollection serviceCollection)
    {
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