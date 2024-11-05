namespace MiturNetWeb.Pages.Usuario;
public partial class EditUsuario : BaseComponentInject
{
    [Parameter]
    public string Id { get; set; }

    //public IEnumerable<string> genero = new string[] { "MASCULINO", "FEMENINO" };   

    MiturNetShared.Model.Operation.UsuarioUpdate _dataUsuario = new();
    protected MiturNetShared.Model.Operation.UsuarioUpdate dataUsuario
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
        var resRoles = await _client.Get<IEnumerable<Roles>>("Account/getRoleList");
        dataRoles = resRoles.Data.ToList().AsQueryable();

        //Response<MiturNetShared.Model.Operation.UsuarioUpdate> res = await _client.Get<MiturNetShared.Model.Operation.UsuarioUpdate>($"Account/getCurrentUserByID?id={Id}");
        //  dataUsuario = res.Data;
        if (!Id.Equals(null) && !Id.Equals(0))
            dataUsuario = (await _client.Get<UsuarioUpdate>($"Account/getCurrentUserByID?id={Id}")).Data;
        else
            dataUsuario = new();
    }
    protected async Task btnSave(UsuarioUpdate args)
    {        
        try
        {              
          var res = await _client.Add<MiturNetShared.Model.Operation.UsuarioUpdate>(args, "Account/updateUser");

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
