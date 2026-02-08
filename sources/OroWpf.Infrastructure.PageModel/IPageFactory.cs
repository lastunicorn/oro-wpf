using System.Windows;

namespace DustInTheWind.OroWpf.Infrastructure.PageModel;

public interface IPageFactory
{
    TView CreatePage<TView, TViewModel>()
        where TView : FrameworkElement
        where TViewModel : PageViewModel;
}