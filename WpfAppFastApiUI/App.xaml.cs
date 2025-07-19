using System.IO;
using System.Windows;
using WpfAppFastApiUI.Services;
using WpfAppFastApiUI.View;

namespace WpfAppFastApiUI
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Window startWindow;

            
            if (File.Exists(UserService.filePath) && !string.IsNullOrWhiteSpace(File.ReadAllText(UserService.filePath)))
            {
                startWindow = new MainWindow();
            }
            else
            {
                startWindow = new RegistrationWindow();
            }

            startWindow.Show();
        }
    }
}