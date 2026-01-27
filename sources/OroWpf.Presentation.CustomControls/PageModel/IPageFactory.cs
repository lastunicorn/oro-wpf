using System.Windows;

namespace DustInTheWind.OroWpf.Presentation.CustomControls.PageModel;

public interface IPageFactory
{
    TView CreatePage<TView, TViewModel>()
        where TView : FrameworkElement
        where TViewModel : PageViewModel;
}