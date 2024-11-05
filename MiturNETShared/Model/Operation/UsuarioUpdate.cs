namespace MiturNetShared.Model.Operation;

public class UsuarioUpdate
{
    //public Roles _roles;
    public string Id { get; set; }
    public string UserName { get; set; }
    public string RoleId { get; set; }
    public string VFirstName { get; set; }
    public string VLastName { get; set; }
    public string? VGender { get; set; }
    //public Roles idRolesNavigation
    //{
    //    get => _roles;
    //    set
    //    {
    //        _roles = value;
    //        RoleId = (string)(_roles?.Id ?? "");
    //    }
    //}
}



