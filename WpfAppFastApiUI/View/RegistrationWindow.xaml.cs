using System.IO;
using System.Windows;
using System.Windows.Input;
using WpfAppFastApiUI.Models;
using WpfAppFastApiUI.Services;

namespace WpfAppFastApiUI.View;

public partial class RegistrationWindow : Window
{
    private MainWindow mainWindow;
    public RegistrationWindow()
    {
        InitializeComponent();
        this.Closed += WindowClose;
    }

    private void LoginLink_Click(object sender, MouseButtonEventArgs e) 
    {
        throw new NotImplementedException();
    }

    private async void Register_Click(object sender, RoutedEventArgs e)
    {
        var user = new UserBase
        {
            name = UserNameBox.Text,
            age = int.Parse(AgeBox.Text),
            phone_number = PhoneNumBox.Text,
            tg_id = TgIdBox.Text
        };
        Session.Login(UserNameBox.Text);
        bool succ=await UserService.RegisterUser(user);
        
        if (mainWindow == null)
        {
            mainWindow=new MainWindow();
            mainWindow.Owner = this;
            mainWindow.Closed+=(_, _)=>mainWindow=null;
        }
        if (succ)
        {
            mainWindow.Show();
            Hide();
        }
    }
    
    private void WindowClose(object sender, EventArgs e)
    {
        Application.Current.Shutdown();
    }
}