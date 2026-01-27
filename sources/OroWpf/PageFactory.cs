using System.Windows;
using DustInTheWind.OroWpf.Presentation.CustomControls.PageModel;
using Microsoft.Extensions.DependencyInjection;

namespace DustInTheWind.OroWpf;

public class PageFactory : IPageFactory
{
    private readonly IServiceProvider serviceProvider;

    public PageFactory(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public TView CreatePage<TView, TViewModel>()
        where TView : FrameworkElement
        where TViewModel : PageViewModel
    {
        TView view = serviceProvider.GetService<TView>();
        view.DataContext = serviceProvider.GetService<TViewModel>();
        return view;
    }
}