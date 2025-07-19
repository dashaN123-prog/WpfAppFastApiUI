using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfAppFastApiUI.Models;

namespace WpfAppFastApiUI.View;

public partial class CategoryWindow : Window
{
    private static readonly HttpClient http = new HttpClient();
    public CategoryWindow(List<Category> categories)
    {
        InitializeComponent();
        CreateCategories(categories);
        this.Closed += WindowClose;
    }

    private void CreateCategories(List<Category> categories)
    {
        for (int i=0; i < categories.Count; i++)
        {
            var btn = new Button
            {
                Content = categories[i].name,
                Width = 200,
                Height = 30,
                FontSize=15,
                Margin = new Thickness(5),
            };
            var i1 = i;
            btn.Click += async (s, e) =>
            {
                var products = await GetProductsByNameAsync(categories[i1].name);
                var productWindow=new ProductWindow(categories[i1].name, products);
                productWindow.Owner = this;
                Hide();
                productWindow.Show();
            };
            if (i<=3)
                CategoryStack1.Children.Add(btn);
            else CategoryStack2.Children.Add(btn);
        }
        var btn2 = new Button
        {
            Content = "Back",
            Width = 200,
            Height = 30,
            FontSize=15,
            Margin = new Thickness(5),
        };
        btn2.Command = CustomCommands.BackToMain;
        CategoryStack.Children.Add(btn2);
    }

    private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        
        this.Hide();
        if (Owner is MainWindow mainWindow) mainWindow.Show();
    }

    private void CommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }
    
    private void WindowClose(object sender, EventArgs e)
    {
        Application.Current.Shutdown();
    }
    
    private async Task<List<Product>> GetProductsByNameAsync(string category)
    {
        try
        {
            var encodedName=Uri.EscapeDataString(category);
            var products = await http.GetFromJsonAsync<List<Product>>($"http://localhost:8000/api/products/{encodedName}");
            return products;
        }
        catch (HttpRequestException ex)
        {
            MessageBox.Show(ex.Message);
            return new List<Product>();
        }
    }
}