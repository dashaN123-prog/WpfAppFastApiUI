namespace WpfAppFastApiUI.Services;

public static class Session
{
    public static string userName { get; set; }
    public static bool isLoggedIn { get; set; }

    public static void Login(string username)
    {
        userName = username;
        isLoggedIn = true;
    }
    
    
}