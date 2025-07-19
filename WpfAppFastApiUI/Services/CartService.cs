using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using WpfAppFastApiUI.Models;

namespace WpfAppFastApiUI.Services;

/// <summary>
/// Сервис для работы с корзиной через API
/// </summary>
public static class CartService
{
    private static readonly HttpClient http = new HttpClient();

    /// <summary>
    /// Добавить товар в корзину
    /// </summary>
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

    /// <summary>
    /// Получить список товаров в корзине пользователя
    /// </summary>
    public static async Task<List<CartProductResponse>> GetCartProductsAsync(int userId)
    {
        
        var response = await http.GetFromJsonAsync<List<CartProductResponse>>(
            $"http://localhost:8000/api/cart/{userId}");

        return response ?? new List<CartProductResponse>();
    }

    /// <summary>
    /// Удалить все товары из корзины пользователя
    /// </summary>
    public static async Task<bool> ClearCartAsync(int userId)
    {
        var response = await http.DeleteAsync($"http://localhost:8000/api/cart/{userId}");
        return response.IsSuccessStatusCode;
    }

    /// <summary>
    /// Удалить конкретный товар из корзины пользователя
    /// </summary>
    public static async Task<bool> DeleteProductFromCartAsync(int userId, int productId)
    {
        var response = await http.DeleteAsync($"http://localhost:8000/api/cart/{userId}/product/{productId}");
        return response.IsSuccessStatusCode;
    }
}