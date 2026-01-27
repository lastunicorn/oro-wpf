using System.Windows;
using DustInTheWind.OroWpf.Controls;

namespace DustInTheWind.OroWpf;

public interface IPageFactory
{
    TView CreatePage<TView, TViewModel>()
        where TView : FrameworkElement
        where TViewModel : PageViewModel;
}