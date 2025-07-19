using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfAppFastApiUI.Models;
using WpfAppFastApiUI.Services;

namespace WpfAppFastApiUI.View;

public partial class CartWindow : Window
{
    public CartWindow(List<CartProductResponse> list)
    {
        InitializeComponent();
        LoadCart(list);
        this.Closed += WindowClose;
    }
    private void WindowClose(object sender, EventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void LoadCart(List<CartProductResponse> list)
{
    CartPanel.Children.Clear();
    double totalPrice = 0;

    foreach (var item in list)
    {
        double itemTotal = item.quantity * item.size_mult * item.product_price;
        totalPrice += itemTotal;

        var border = new Border
        {
            Margin = new Thickness(10),
            Background = Brushes.White,
            CornerRadius = new CornerRadius(5),
            BorderBrush = Brushes.LightGray,
            BorderThickness = new Thickness(1),
        };

        var stack = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Margin = new Thickness(10)
        };

        var textName = new TextBlock
        {
            Text = item.product_name,
            FontSize = 22,
            Margin = new Thickness(0, 10, 10, 10),
            FontWeight = FontWeights.Bold,
            Width = 200
        };
        var textPrice = new TextBlock
        {
            Text = item.product_price.ToString("F2"),
            FontSize = 15,
            Margin = new Thickness(0, 10, 10, 10),
            FontWeight = FontWeights.Bold,
            Width = 60
        };
        var textCount = new TextBlock
        {
            Text = item.quantity.ToString(),
            FontSize = 15,
            Margin = new Thickness(0, 10, 10, 10),
            FontWeight = FontWeights.Bold,
            Width = 40
        };
        var textSize = new TextBlock
        {
            Text = item.size,
            FontSize = 15,
            Margin = new Thickness(0, 10, 10, 10),
            FontWeight = FontWeights.Bold,
            Width = 80
        };
        var textTotal = new TextBlock
        {
            Text = itemTotal.ToString("F2"),
            FontSize = 22,
            Margin = new Thickness(0, 10, 10, 10),
            FontWeight = FontWeights.Bold,
            Width = 80
        };

        // Кнопка Удалить
        var deleteButton = new Button
        {
            Content = "Удалить",
            Width = 75,
            Margin = new Thickness(10, 0, 0, 0),
            Tag = item  // Передаем объект товара через Tag
        };
        deleteButton.Click += async (s, e) =>
        {
            var button = s as Button;
            var cartItem = button.Tag as CartProductResponse;
            if (cartItem == null) return;

            try
            {
                var userId = int.Parse(File.ReadAllText(UserService.filePath));
                // Здесь нужно реализовать метод удаления одного товара по userId и productId
                bool success = await CartService.DeleteProductFromCartAsync(userId, cartItem.product_id);

                if (success)
                {
                    var updatedCart = await CartService.GetCartProductsAsync(userId);
                    LoadCart(updatedCart);
                }
                else
                {
                    MessageBox.Show("Не удалось удалить товар");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении товара: {ex.Message}");
            }
        };

        stack.Children.Add(textName);
        stack.Children.Add(textPrice);
        stack.Children.Add(textCount);
        stack.Children.Add(textSize);
        stack.Children.Add(textTotal);
        stack.Children.Add(deleteButton);

        border.Child = stack;
        CartPanel.Children.Add(border);
    }

    TotalBlock.Text = $"Total price: {totalPrice:F2}";
}


    private async void ButtonDelete_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            var userId = int.Parse(File.ReadAllText(UserService.filePath));
            bool isSuccess = await CartService.DeleteCartAsync(userId);

            if (isSuccess)
            {
                CartPanel.Children.Clear();
                TotalBlock.Text = "Total price: 0.00";
                MessageBox.Show("Корзина очищена!");
            }
            else
            {
                MessageBox.Show("Не удалось очистить корзину");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при очистке корзины: {ex.Message}");
        }
    }

    private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        
        this.Hide();

        if (Owner is MainWindow mainWindow)
            mainWindow.Show();
        else if (Owner is ProductWindow productWindow)
            productWindow.Show();
    }

    private void CommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        
        e.CanExecute = true;
    }
}
