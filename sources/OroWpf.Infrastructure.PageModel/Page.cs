namespace DustInTheWind.OroWpf.Infrastructure.PageModel;

public class Page
{
    public string Id { get; init; }

    public Type ViewType { get; init; }
    
    public Type ViewModelType { get; init; }
}