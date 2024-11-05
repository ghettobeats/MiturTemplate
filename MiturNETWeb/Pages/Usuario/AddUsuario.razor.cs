namespace MiturNetWeb.Pages.Usuario;
public partial class AddUsuario : BaseComponentInject
{
    public IEnumerable<string> genero = new string[] { "MASCULINO", "FEMENINO" };   

    MiturNetShared.Model.Operation.UsuarioRegister _dataUsuario;
    protected MiturNetShared.Model.Operation.UsuarioRegister dataUsuario
    {
        get
        {
            return _dataUsuario;
        }
        set
        {
            if (!object.Equals(_dataUsuario, value))
            {
                var args = new PropertyChangedEventArgs() { Name = "dataUsuario", NewValue = value, OldValue = _dataUsuario };
                _dataUsuario = value;
                OnPropertyChanged(args);
                InvokeAsync(StateHasChanged);
            }
        }
    }


   IEnumerable<MiturNetShared.Model.Operation.Roles> _dataRoles;
    protected IEnumerable<MiturNetShared.Model.Operation.Roles> dataRoles
    {
        get
        {
            return _dataRoles;
        }
        set
        {
            if (!object.Equals(_dataRoles, value))
            {
                var args = new PropertyChangedEventArgs() { Name = "dataRoles", NewValue = value, OldValue = _dataRoles };
                _dataRoles = value;
                OnPropertyChanged(args);
                Reload();
            }
        }
    }

    protected override async Task OnInitializedAsync()
    {
        var resRoles = await _client.Get<IEnumerable<MiturNetShared.Model.Operation.Roles>>("Account/getRoleList");
        dataRoles = resRoles.Data.ToList().AsQueryable();

            dataUsuario = new();
    }
    protected async Task btnSave(MiturNetShared.Model.Operation.UsuarioRegister args)
    {        
        try
        {              
          var res = await _client.Add<MiturNetShared.Model.Operation.UsuarioRegister>(args, "Account/saveUser");

            DialogService.Close(dataUsuario);
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
