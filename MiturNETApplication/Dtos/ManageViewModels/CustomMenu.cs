namespace MiturNetApplication.Dtos.ManageViewModels;
public class CustomMenu
{
    public string vMenuID { get; set; }
    public string NameWithParent { get; set; }
    public string nvMenuName { get; set; }
    public int iSerialNo { get; set; }
    public string nvFabIcon { get; set; }
    public string vParentMenuID { get; set; }
    public string nvPageUrl { get; set; }
    public List<CustomMenu> Child { get; set; }
}