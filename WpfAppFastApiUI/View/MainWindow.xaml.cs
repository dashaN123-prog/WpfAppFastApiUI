using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfAppFastApiUI.Models;
using WpfAppFastApiUI.Services;
using WpfAppFastApiUI.View;

namespace WpfAppFastApiUI;

public partial class MainWindow : Window
{
    private static readonly HttpClient http = new HttpClient();

    private Task<List<Category>> _categories;
    private CategoryWindow _categoryWindow;
    private CartWindow _cartWindow;
    private DiscountCardWindow _discountCardWindow;

    public MainWindow()
    {
        InitializeComponent();
        _categories = LoadCategoryAsync();
        this.Closed += WindowClose;
    }

    private async Task<List<Category>> LoadCategoryAsync()
    {
        try
        {
            var categories = await http.GetFromJsonAsync<List<Category>>("http://localhost:8000/api/categories");
            return categories;
        }
        catch (HttpRequestException ex)
        {
            MessageBox.Show($"Ошибка загрузки категорий: {ex.Message}");
            return new List<Category>();
        }
    }

    private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        if (_categoryWindow == null)
        {
            _categoryWindow = new CategoryWindow(_categories.Result);
            _categoryWindow.Owner = this;
            _categoryWindow.Closed += (_, _) => _categoryWindow = null;
        }

        _categoryWindow.Show();
        this.Hide();
    }

    private void CommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    private async void CommandBindingCart_OnExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        try
        {
            if (!File.Exists(UserService.filePath))
            {
                MessageBox.Show("Файл с ID пользователя не найден.");
                return;
            }

            if (!int.TryParse(File.ReadAllText(UserService.filePath), out int userId))
            {
                MessageBox.Show("Некорректный формат ID пользователя.");
                return;
            }

            var cart = await CartService.GetCartProductsAsync(userId);

            if (cart == null || cart.Count == 0)
            {
                MessageBox.Show("Корзина пуста или не удалось загрузить данные.");
                return;
            }

            if (_cartWindow == null)
            {
                _cartWindow = new CartWindow(cart);
                _cartWindow.Owner = this;
                _cartWindow.Closed += (_, _) => _cartWindow = null;
            }
            else
            {
                _cartWindow.Focus();
            }

            _cartWindow.Show();
            this.Hide();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка загрузки корзины: {ex.Message}");
        }
    }

    private void WindowClose(object sender, EventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void DiscountCard_Click(object sender, RoutedEventArgs e)
    {
        if (_discountCardWindow == null)
        {
            _discountCardWindow = new DiscountCardWindow();
            _discountCardWindow.Owner = this;
            _discountCardWindow.Closed += (_, _) => _discountCardWindow = null;
        }

        _discountCardWindow.Show();
        this.Hide();
    }
}
