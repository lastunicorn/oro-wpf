using System.Windows;
using DustInTheWind.OroWpf.Presentation.Controls;

namespace DustInTheWind.OroWpf.Presentation;

public interface IPageFactory
{
    TView CreatePage<TView, TViewModel>()
        where TView : FrameworkElement
        where TViewModel : PageViewModel;
}