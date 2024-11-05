namespace MiturNetWeb.Pages.Usuario;
public partial class Usuario : BaseComponentInject
{    
    protected RadzenDataGrid<MiturNetShared.Model.Operation.Usuario> dbGridUsuario;
    protected Response<IEnumerable<MiturNetShared.Model.Operation.Usuario>> response { get; set; }

    IEnumerable<MiturNetShared.Model.Operation.Usuario> _dataUsuario;
    protected IEnumerable<MiturNetShared.Model.Operation.Usuario> dataUsuario
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
                InvokeAsync(() => { StateHasChanged(); }); ;
            }
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadData();
    }
    public async Task LoadData()
    {
        isLoading = true;
        response = await _client.Get<IEnumerable<MiturNetShared.Model.Operation.Usuario>>("Account/getUserList");
        if (response.Data == null)
        {
            //loading = true;
        }
        else
        {
            dataUsuario = response.Data.ToList().AsQueryable();
            isLoading = false;
            await dbGridUsuario.Reload();

        }
        await InvokeAsync(() => { StateHasChanged(); });
    }
    protected async Task btnAdd(MouseEventArgs args)
    {
        var dialogResult = await DialogService.OpenAsync<AddUsuario>("Usuario", 
                new Dictionary<string, object>() { { "Id", 0 } },
                new DialogOptions() { Draggable = true });
        await LoadData();
        await dbGridUsuario.Reload();
        await InvokeAsync(() => { StateHasChanged(); });
    }
    protected async Task btnEdit(MouseEventArgs args, dynamic data)
    {
        var dialogResult = await DialogService.OpenAsync<EditUsuario>("Usuario", 
            new Dictionary<string, object>() { { "Id", data.Id } });
        await LoadData();
        await dbGridUsuario.Reload();
        await InvokeAsync(() => { StateHasChanged(); });
    }
    protected async Task btnDelete(MouseEventArgs args, dynamic data)
    {
        try
        {
            var result = await DialogService.OpenAsync<DialogDelete>("Eliminar Registro",
                new Dictionary<string, object> {
                    {"TitleOne", "Email: " },
                    {"TitleFieldOne", data.Email},
                    {"TitleTwo", "Nombre: " },
                    {"TitleFieldTwo",data.FullName },
                });

            if (result)
            {
                var res = await _client.Delete($"Usuario/{data.Id}");
                if (res.Data != false)
                {
                    await LoadData();
                    await dbGridUsuario.Reload();
                }
            }
        }
        catch (System.Exception crmDeleteTaskException)
        {
            NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"No fue posible eliminar el registro" });
        }
    }

    protected async Task btnResetPassowrd(MouseEventArgs args, dynamic data)
    {
        var dialogResult = await DialogService.OpenAsync<ResetUsuario>("Contraseña Usuario", new Dictionary<string, object>() { { "id", data.Id } });
        await LoadData();
        await dbGridUsuario.Reload();
        await InvokeAsync(() => { StateHasChanged(); });

        //try
        //{
        //    var result = await DialogService.OpenAsync<DialogDelete>("Reset Passowrd",
        //        new Dictionary<string, object> {
        //            {"TitleOne", "Email: " },
        //            {"TitleFieldOne", data.Email},
        //            {"TitleTwo", "Nombre: " },
        //            {"TitleFieldTwo",data.FullName },
        //        });

        //    if (result)
        //    {
        //        var res = await _client.Delete($"Usuario/{data.id}");
        //        if (res.Data != false)
        //        {
        //            await LoadData();
        //            await dbGridUsuario.Reload();
        //        }
        //    }
        //}
        //catch (System.Exception crmDeleteTaskException)
        //{
        //    NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"No fue posible eliminar el registro" });
        //}
    }
    

    protected async Task btnExportar(RadzenSplitButtonItem args)
    {
        if (args?.Value == "xlsx")
        {
            _client.Exportar("Usuario", "excel", new Query() { OrderBy = dbGridUsuario.Query.OrderBy, Filter = dbGridUsuario.Query.Filter });
        }
    }
}
