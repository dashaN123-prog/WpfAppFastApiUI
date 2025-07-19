namespace WpfAppFastApiUI.Models;

public class AddToCartRequest
{
    public int user_id { get; set; }
    public int prod_id { get; set; }
    public int quant { get; set; }
    public string size { get; set; }
}