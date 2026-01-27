using System.Windows;
using DustInTheWind.OroWpf.Presentation;
using Microsoft.Extensions.DependencyInjection;

namespace DustInTheWind.OroWpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        IServiceProvider serviceProvider = ConfigureServices();
        MainWindow = CreateAndShoeMainWindow(serviceProvider);

        base.OnStartup(e);
    }

    private static IServiceProvider ConfigureServices()
    {
        ServiceCollection serviceCollection = new();
        Setup.ConfigureServices(serviceCollection);
        return serviceCollection.BuildServiceProvider();
    }

    private static MainWindow CreateAndShoeMainWindow(IServiceProvider serviceProvider)
    {
        MainWindow mainWindow = serviceProvider.GetService<MainWindow>();
        mainWindow.DataContext = serviceProvider.GetService<MainViewModel>();

        mainWindow.Show();

        return mainWindow;
    }
}
