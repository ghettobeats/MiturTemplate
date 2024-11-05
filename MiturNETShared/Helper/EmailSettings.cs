namespace MiturNetShared.Helper;

public class EmailSettings
{
    public string Username { get; set; }
    public string Password { get; set; }
    public int Port { get; set; }
    public string FromEmail { get; set; }
    public string Host { get; set; }

    //public EmailSettings()
    //{
    //    this.Username = "no_reply@segasa.com.do";
    //    this.Password = "N0_R3ply3838";
    //    this.Port = 587;
    //    this.FromEmail = "no_reply@segasa.com.do";
    //    this.Host = "smtp.office365.com";
    //}


    public EmailSettings()
    {
        this.Username = "notificaciones@segasa.com.do";
        this.Password = "N0_R3ply3838";
        this.Port = 587;
        this.FromEmail = "notificaciones@segasa.com.do";
        this.Host = "smtp.office365.com";
    }
}