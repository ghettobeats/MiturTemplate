namespace MiturNetWeb.Pages.Usuario;
public partial class LoginUsuario : BaseComponentInject
{
    [Parameter]
    public string id { get; set; }

    [Inject]
    public SecurityService securityService { get; set; }

    MiturNetShared.Model.Operation.AccountLogin _dataLoginUsuario;
    protected MiturNetShared.Model.Operation.AccountLogin dataLoginUsuario
    {
        get
        {
            return _dataLoginUsuario;
        }
        set
        {
            if (!object.Equals(_dataLoginUsuario, value))
            {
                var args = new PropertyChangedEventArgs() { Name = "dataLoginUsuario", NewValue = value, OldValue = _dataLoginUsuario };
                _dataLoginUsuario = value;
                OnPropertyChanged(args);
                InvokeAsync(StateHasChanged);
            }
        }
    }
    protected override async Task OnInitializedAsync()
    {
        dataLoginUsuario = new();
        dataLoginUsuario.RememberMe = true;
    }
    protected async Task btnSave(AccountLogin args) //usted no manda a guardar si no a logear.
    {
        try //esto no tiene sentido.. el try
        {
            var resultado = await securityService.Login(args);

            if (resultado.Succes)
            {
                NavigationManager.NavigateTo("/");
            }
            else
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = resultado.Message,
                    Detail = "Aquí el Error!!!",
                    Duration = 40000
                });
            }
        }
        catch (Exception UsuarioException)
        {
            NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to create new Usuario!" });
        }
    }
}
