using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using WpfAppFastApiUI.Models;

namespace WpfAppFastApiUI.Services;

public static class UserService
{
    public static readonly HttpClient http = new HttpClient();
    
    public static readonly string filePath = "userdata.txt";

    public static async Task<bool> RegisterUser(UserBase user)
    {
        try
        {
            var response = await http.PostAsJsonAsync("http://localhost:8000/api/users", user);
            if (response.IsSuccessStatusCode)
            {
                var userdata=await response.Content.ReadFromJsonAsync<UserBase>();
                await File.WriteAllTextAsync(filePath,userdata.id.ToString());
            }
            return response.IsSuccessStatusCode;
        }
        catch(HttpRequestException e)
        {
            MessageBox.Show("error");
            return false;
        }
    }
    public static async Task<UserBase> GetUserAsync(int userId)
    {
        using var http = new HttpClient();
        return await http.GetFromJsonAsync<UserBase>($"http://localhost:8000/api/users/{userId}");
    }
}