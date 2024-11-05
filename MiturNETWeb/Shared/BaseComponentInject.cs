using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Components.Authorization;
using MiturNetWeb.Services;
using System.Security.Claims;

namespace MiturNetWeb.Shared;
public partial class BaseComponentInject : LayoutComponentBase
{
    protected string pagingSummaryFormat = "Mostrando página {0} de {1} (total registro(s) {2})";
    protected IEnumerable<int> pageSizeOptions = new int[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 150, 200, 300, 400, 500 };
    protected string pageSizeText = "";
    protected bool showPagerSummary = true;
    protected int pageSize = 10;
    protected bool isLoading = false;
    protected bool isBusySaving = false;

    protected string exportedTrueDir = @"Datos exportados.";
    protected string exportedFalseDir = @"Error al intentar exportar los Datos.";

    protected string emptyText = "No hay registros para mostrar.";
    protected string gridGroupPanelText = "Arrastre un encabezado de columna aquí y suéltelo para agruparlo por esa columna";

    protected string filterAndOperatorText = "Y";
    protected string filterApplyFilterText = "Filtrar";
    protected string filterClearFilterText = "Quitar Filtro";
    protected string filterContainsText = "Contiene";
    protected string filterEndsWithText = "Termina con";
    protected string filterEqualsText = "Igual";
    protected string filterGreaterThanOrEqualsText = "Mayor que o igual";
    protected string filterGreaterThanText = "Mayor que";
    protected string filterIsNotNullText = "No es nulo";
    protected string filterIsNullText = "Es nulo";
    protected string filterLessThanOrEqualsText = "Menor que o igual";
    protected string filterLessThanText = " Menor que";
    protected string filterNotEqualsText = "No es igual";
    protected string filterOrOperatorText = "O";
    protected string filterStartsWithText = "Comienza con";

    public void Reload()
    {
        InvokeAsync(StateHasChanged);
    }

    //[Inject]
    //protected MailService _mailService { get; set; }

    [Inject]
    protected ILocalStorage _localStorageSession { get; set; }

    [Inject]
    private HubConnectionBuilder _hubConnectionBuilder { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }
    public Dictionary<string, object> AdditionalAttributes { get; set; }

    [Parameter]
    public string Value { get; set; }

    [CascadingParameter]
    protected Task<AuthenticationState> _authentication { get; set; }

    protected ClaimsPrincipal _claimsPrincipal { get; set; }

    [Inject]
    protected SecurityService _securityService { get; set; }

    [Inject]
    protected SecurityServiceOdoo _securityServiceOdoo { get; set; }

    [Inject]
    protected NavigationManager UriHelper { get; set; }

    [Inject]
    public DialogService DialogService { get; set; }

    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    [Inject]
    protected NotificationService NotificationService { get; set; }

    [Inject]
    protected ExportServices servicesExport { get; set; }

    [Inject]
    protected ExportToFile exportToFile { get; set; }

    [Inject]
    public IBaseHttpClient _client { get; set; }

    [Inject]
    public IBaseHttpClientOdoo _clientOdoo { get; set; }

    public void OnPropertyChanged(PropertyChangedEventArgs args)
    {

    }

    public async Task<dynamic> ShowDialog<T>(string title, bool showTitle, bool dragged,
        string with = null, string height = null,
        Dictionary<string, object> paramList = null) where T : ComponentBase
    {
        var result = await DialogService.OpenAsync<T>(title,
            paramList,
            new DialogOptions()
            {
                ShowTitle = showTitle,
                Draggable = dragged,
                Width = with,
                Height = height,
                ShowClose = true
            });

        return result;
    }

    public void ShowNotification(NotificationSeverity notificationSeverity, string summary, string detail)
    {
        NotificationService.Notify(
             new NotificationMessage()
             {
                 Severity = notificationSeverity,
                 Summary = summary,
                 Detail = detail,
                 Duration = 4000
             });
    }

    public async Task<string> FormatDateOnly(DateTime dateTime)
    {
        var ToDateOnly = dateTime.Date.ToString("MM/dd/yyyy");
        return ToDateOnly;
    }

    public async Task<string> getUserID()
    {
        var Usuario = (await _authentication).User.Claims.ToList();
        return Usuario[1].Value;
    }

  


}

