namespace WpfAppFastApiUI.Models;

public class Product
{
    public int id{get;set;}
    public string name { get; set; }
    public int price { get; set; }
    public int stock { get; set; }
    public string description { get; set; }
    public string img_url { get; set; }
    public int age_restriction { get; set; }
    public int category_id { get; set; }
}