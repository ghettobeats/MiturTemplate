

namespace MiturNetWeb.Layouts;

public partial class MainLayoutComponent : LayoutComponentBase
{
       
    [Inject]
    protected NavigationManager UriHelper { get; set; }

    [Inject]
    protected DialogService DialogService { get; set; }

    [Inject]
    protected TooltipService TooltipService { get; set; }

    [Inject]
    protected ContextMenuService ContextMenuService { get; set; }

    [Inject]
    protected NotificationService NotificationService { get; set; }

    [Inject]
    protected SecurityService SecurityService { get; set; }

    protected RadzenBody body0;
    protected RadzenSidebar sidebar0;

 

}
