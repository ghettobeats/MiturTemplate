namespace MiturNetWeb.Pages.TipoPuesto;
public partial class TipoPuestoComponent : BaseComponentInject
{
    public bool loading;

    public RadzenDataGrid<MiturNetShared.Model.Operation.TipoPuesto> dataGrid;    

    IEnumerable<MiturNetShared.Model.Operation.TipoPuesto> _getTipoPuestosResult;
    protected IEnumerable<MiturNetShared.Model.Operation.TipoPuesto> getTipoPuestosResult
    {
        get
        {
            return _getTipoPuestosResult;
        }
        set
        {
            if (!object.Equals(_getTipoPuestosResult, value))
            {
                var args = new PropertyChangedEventArgs() { Name = "getTipoPuestosResult", NewValue = value, OldValue = _getTipoPuestosResult };
                _getTipoPuestosResult = value;
                OnPropertyChanged(args);
                InvokeAsync(() => { StateHasChanged(); }); ;
            }
        }
    }
    protected Response<IEnumerable<MiturNetShared.Model.Operation.TipoPuesto>> res { get; set; }
    protected override async Task OnInitializedAsync()
    {
        await LoadData();
    }
    public async Task LoadData()
    {
        res = await _client.Get<IEnumerable<MiturNetShared.Model.Operation.TipoPuesto>>("TipoPuesto");
        if (res.Data == null)
        {
            loading = true;
        }
        else
        {
            getTipoPuestosResult = res.Data.ToList();
            loading = false;
            await dataGrid.Reload();

        }
        await InvokeAsync(() => { StateHasChanged(); });
    }

    public async Task Button0Click(MouseEventArgs args)
    {
        var dialogResult = await DialogService.OpenAsync<AddTipoPuesto>("Agregar Tipo Puesto", new Dictionary<string, object>() { { "id", 0 } });
        await LoadData();
        await dataGrid.Reload();
        await InvokeAsync(() => { StateHasChanged(); });
    }
    public async Task GridEditButtonClick(MouseEventArgs args, dynamic data)
    {
        var dialogResult = await DialogService.OpenAsync<AddTipoPuesto>("Editar Tipo Puesto", new Dictionary<string, object>() { { "id", data.id } });
        await LoadData();
        await dataGrid.Reload();
        await InvokeAsync(() => { StateHasChanged(); });
    }
    public async Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
    {
        try
        {
            var res = await _client.Delete($"TipoPuesto/{data.id}");
            if (res.Data != false)
            {
                await LoadData();
                await dataGrid.Reload();
            }
        }
        catch (System.Exception crmDeleteTaskException)
        {
            NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"No fue posible eliminar el registro" });
        }
        DialogService.Close(null);
    }

    protected async System.Threading.Tasks.Task Splitbutton0Click(RadzenSplitButtonItem args)
    {
        //if (args?.Value == "csv")
        //{
        //    await SegasaMrpBlazor.ExportTipoPuestosToCSV(new Query() { Filter = $@"{grid0.Query.Filter}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "Estatus", Select = "idTipoPuesto,Estatus.Descripcion,Codigo,Descripcion,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate" }, $"Tipo Puesto");

        //}

        //if (args == null || args.Value == "xlsx")
        //{
        //    await exportService.ExportTipoPuestosToExcel(new Query() { Filter = $@"{getTipoPuestosResult.AsQueryable.Filter}", OrderBy = $"{getTipoPuestosResult.AsQueryable.OrderBy}", Select = "idTipoPuesto,Estatus.Descripcion,Codigo,Descripcion" }, $"Tipo Puesto");

        //}
    }
}
