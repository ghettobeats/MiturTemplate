namespace MiturNetShared.Model.Odoo;

public class OdooToken
{
     public int uid { get; set; }
    public UserContext user_context { get; set; }
    public int company_id { get; set; }
    public IList<int> company_ids { get; set; }
    public int partner_id { get; set; }
    public string access_token { get; set; }
    public bool company_name { get; set; }
    public string currency { get; set; }
    public string country { get; set; }
    public string contact_address { get; set; }
    public int customer_rank { get; set; }
}