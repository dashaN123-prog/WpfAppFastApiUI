using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using WpfAppFastApiUI.Models;

namespace WpfAppFastApiUI.Services;

public static class CartService
{
   
    private static readonly HttpClient http = new HttpClient();
    
    public static async Task<bool> AddToCartAsync(int userId, int productId, int quantity, string size)
    {
        var payload = new AddToCartRequest
        {
            user_id = userId,
            prod_id = productId,
            quant = quantity,
            size = size
        };

        var response = await http.PostAsJsonAsync("http://localhost:8000/api/cart/", payload);
        return response.IsSuccessStatusCode;
    }
    
    public static async Task<List<CartProductResponse>> GetCartProductsAsync(int userId)
    {
        var response = await http.GetFromJsonAsync<List<CartProductResponse>>(
            $"http://localhost:8000/api/cart/{userId}");

        return response ?? new List<CartProductResponse>();
    }
    
    public static async Task<bool> DeleteCartAsync(int userId)
    {
        var response = await http.DeleteAsync($"http://localhost:8000/api/cart/{userId}");
        return response.IsSuccessStatusCode;
    }
    
    public static async Task<bool> DeleteProductFromCartAsync(int userId, int productId)
    {
        var response = await http.DeleteAsync($"http://localhost:8000/api/cart/{userId}/product/{productId}");
        return response.IsSuccessStatusCode;
    }


}