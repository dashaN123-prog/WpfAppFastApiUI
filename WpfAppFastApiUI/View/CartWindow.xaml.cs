using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfAppFastApiUI.Models;
using WpfAppFastApiUI.Services;

namespace WpfAppFastApiUI.View
{
    public partial class CartWindow : Window
    {
        private int _userId;

        public CartWindow(List<CartProductResponse> list)
        {
            InitializeComponent();
            _userId = int.Parse(File.ReadAllText(UserService.filePath));
            LoadCart(list);
            this.Closed += WindowClose;
        }

        private void WindowClose(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private async Task ReloadAsync()
        {
            var list = await CartService.GetCartProductsAsync(_userId);
            LoadCart(list);
        }

        private void LoadCart(List<CartProductResponse> list)
        {
            CartPanel.Children.Clear();
            double total = 0;

            foreach (var item in list)
            {
                total += item.product_price * item.quantity * item.size_mult;

                var stack = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(10)
                };

                stack.Children.Add(new TextBlock
                {
                    Text = item.product_name,
                    Width = 200,
                    Margin = new Thickness(10)
                });

                stack.Children.Add(new TextBlock
                {
                    Text = $"Размер: {item.size}",
                    Width = 100,
                    Margin = new Thickness(10)
                });

                stack.Children.Add(new TextBlock
                {
                    Text = $"Кол-во: {item.quantity}",
                    Width = 100,
                    Margin = new Thickness(10)
                });

                stack.Children.Add(new TextBlock
                {
                    Text = $"Цена: {item.product_price}₽",
                    Width = 100,
                    Margin = new Thickness(10)
                });

                var deleteButton = new Button
                {
                    Content = "Удалить",
                    Width = 100,
                    Tag = item,
                    Margin = new Thickness(10)
                };

                deleteButton.Click += async (s, e) =>
                {
                    var cartItem = (s as Button)?.Tag as CartProductResponse;
                    if (cartItem == null)
                        return;

                    bool result = await CartService.DeleteProductFromCartAsync(_userId, cartItem.product_id);
                    if (result)
                        await ReloadAsync();
                    else
                        MessageBox.Show("Не удалось удалить товар", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                };

                stack.Children.Add(deleteButton);

                var border = new Border
                {
                    BorderThickness = new Thickness(1),
                    BorderBrush = System.Windows.Media.Brushes.Gray,
                    Child = stack,
                    Margin = new Thickness(5)
                };

                CartPanel.Children.Add(border);
            }

            TotalBlock.Text = $"Total price: {total:F2}₽";
        }

        private async void ButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите очистить корзину?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                bool success = await CartService.ClearCartAsync(_userId);
                if (success)
                {
                    await ReloadAsync();
                    MessageBox.Show("Корзина очищена.", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Ошибка при очистке корзины.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void CommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
