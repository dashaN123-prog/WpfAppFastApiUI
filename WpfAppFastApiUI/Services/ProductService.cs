using System.Net.Http;
using System.Net.Http.Json;
using WpfAppFastApiUI.Models;

namespace WpfAppFastApiUI.Services;

public static class ProductService
{
    public static readonly HttpClient Http = new HttpClient();
    
    public static async Task<List<Size>> GetSizesAsync()
    {
        var response=await Http.GetFromJsonAsync<List<Size>>($"http://localhost:8000/api/size/");
        return response ?? new List<Size>();
    }
}