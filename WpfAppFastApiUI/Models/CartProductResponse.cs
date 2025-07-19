namespace WpfAppFastApiUI.Models;

public class CartProductResponse
{
    public int product_id { get; set; }
    public string product_name { get; set; }
    public int product_price { get; set; }
    public string size { get; set; }
    public double size_mult { get; set; }
    public int quantity { get; set; }
}
