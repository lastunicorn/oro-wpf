using System.Windows;
using DustInTheWind.OroWpf.Infrastructure.Jobs;
using DustInTheWind.OroWpf.Presentation;
using Microsoft.Extensions.DependencyInjection;

namespace DustInTheWind.OroWpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private IServiceProvider serviceProvider;

    protected override void OnStartup(StartupEventArgs e)
    {
        serviceProvider = ConfigureServices();
        MainWindow = CreateAndShowMainWindow(serviceProvider);

        CreateAndStartJobs();

        base.OnStartup(e);
    }

    private static IServiceProvider ConfigureServices()
    {
        ServiceCollection serviceCollection = new();
        Setup.ConfigureServices(serviceCollection);
        return serviceCollection.BuildServiceProvider();
    }

    private static MainWindow CreateAndShowMainWindow(IServiceProvider serviceProvider)
    {
        MainWindow mainWindow = serviceProvider.GetService<MainWindow>();
        mainWindow.DataContext = serviceProvider.GetService<MainViewModel>();

        mainWindow.Show();

        return mainWindow;
    }

    private void CreateAndStartJobs()
    {
        JobEngine jobEngine = serviceProvider.GetService<JobEngine>();

        IEnumerable<IJob> jobs = serviceProvider.GetServices<IJob>();
        jobEngine.AddRange(jobs);

        jobEngine.Start();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        JobEngine jobEngine = serviceProvider.GetService<JobEngine>();
        jobEngine.Stop();

        base.OnExit(e);
    }
}
