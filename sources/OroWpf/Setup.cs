using System.Reflection;
using DustInTheWind.ClockWpf.Templates;
using DustInTheWind.OroWpf.Infrastructure.Jobs;
using DustInTheWind.OroWpf.Infrastructure.PageModel;
using DustInTheWind.OroWpf.Jobs;
using DustInTheWind.OroWpf.Ports.SettingsAccess;
using DustInTheWind.OroWpf.Presentation;
using DustInTheWind.OroWpf.Presentation.Controls;
using DustInTheWind.OroWpf.Presentation.Controls.About;
using DustInTheWind.OroWpf.Presentation.Controls.Settings;
using DustInTheWind.OroWpf.Presentation.Controls.Templates;
using Microsoft.Extensions.DependencyInjection;

namespace DustInTheWind.OroWpf;

internal static class Setup
{
    public static void ConfigureServices(IServiceCollection serviceCollection)
    {
        ISettings settings = new Settings();
        serviceCollection.AddSingleton(settings);

        ApplicationState applicationState = CreateApplicationState(settings);
        serviceCollection.AddSingleton(applicationState);

        PageEngine pageEngine = CreatePageEngine();
        serviceCollection.AddSingleton(pageEngine);
        serviceCollection.AddSingleton<IPageFactory, PageFactory>();

        serviceCollection.AddJobsFromAssemblyContaining<ClockTemplateChangedJob>();

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

    private static ApplicationState CreateApplicationState(ISettings settings)
    {
        IEnumerable<Assembly> assemblies = LoatTemplateAssemblies();

        List<Type> templateTypes = EnumerateClockTemplates(assemblies)
            .ToList();

        ApplicationState applicationState = new()
        {
            AvailableTemplateTypes = templateTypes
        };

        if (templateTypes?.Count > 0)
        {
            Type selectedTemplateType = LoadTemplateTypeFromSettings(settings, templateTypes);

            applicationState.ClockTemplate = (ClockTemplate)Activator.CreateInstance(selectedTemplateType);
        }

        return applicationState;
    }

    private static IEnumerable<Assembly> LoatTemplateAssemblies()
    {
        yield return typeof(DefaultTemplate).Assembly;

        foreach (Assembly assembly in PluginSupport.LoatTemplateAssemblies())
            yield return assembly;
    }

    private static Type LoadTemplateTypeFromSettings(ISettings settings, List<Type> templateTypes)
    {
        string savedTemplateTypeName = settings.ClockTemplateType;

        if (!string.IsNullOrEmpty(savedTemplateTypeName))
        {
            Type savedTemplateType = templateTypes
                .FirstOrDefault(x => x.FullName == savedTemplateTypeName || x.Name == savedTemplateTypeName);

            if (savedTemplateType != null)
                return savedTemplateType;
        }

        return templateTypes.FirstOrDefault(x => x == typeof(DefaultTemplate)) ?? templateTypes.First();
    }

    private static IEnumerable<Type> EnumerateClockTemplates(IEnumerable<Assembly> assemblies)
    {
        return assemblies
            .SelectMany(x => x.GetTypes())
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
