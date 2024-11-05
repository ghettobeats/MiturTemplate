namespace MiturNetWeb.Shared;
public partial class DialogDelete
{
    [Parameter]
    public string TitleOne { get; set; }

    [Parameter]
    public string TitleFieldOne { get; set; }

    [Parameter]
    public string TitleTwo { get; set; }

    [Parameter]
    public string TitleFieldTwo { get; set; }


    protected async Task CloseDialog()
    {
        DialogService.Close(false);
    }

    protected async Task Accept()
    {
        DialogService.Close(true);
    }
}
