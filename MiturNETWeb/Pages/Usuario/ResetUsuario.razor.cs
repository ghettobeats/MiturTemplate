namespace MiturNetWeb.Pages.Usuario;
public partial class ResetUsuario : BaseComponentInject
{
    [Parameter]
    public string id { get; set; }

    MiturNetShared.Model.Operation.AccountResetPassword _dataResetUsuario;
    protected MiturNetShared.Model.Operation.AccountResetPassword dataResetUsuario
    {
        get
        {
            return _dataResetUsuario;
        }
        set
        {
            if (!object.Equals(_dataResetUsuario, value))
            {
                var args = new PropertyChangedEventArgs() { Name = "dataResetUsuario", NewValue = value, OldValue = _dataResetUsuario };
                _dataResetUsuario = value;
                OnPropertyChanged(args);
                InvokeAsync(StateHasChanged);
            }
        }
    }

    protected override async Task OnInitializedAsync()
    {
        dataResetUsuario = new();
        dataResetUsuario.id = id;
    }
    protected async Task btnSave(MiturNetShared.Model.Operation.AccountResetPassword args)
    {
        try
        {           
            if ((await _client.Add<MiturNetShared.Model.Operation.AccountResetPassword>(args, "Account/resetpassword")).Succes)
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = $"Cambiado", Detail = $"La Contraseña fue cambiada!" });

            DialogService.Close(dataResetUsuario);

        }
        catch (System.Exception UsuarioException)
        {
            NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to create new Usuario!" });
        }
    }

    protected async Task btnCancel(MouseEventArgs args)
    {
        DialogService.Close(null);
    }
}
