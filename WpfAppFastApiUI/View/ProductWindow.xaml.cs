using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfAppFastApiUI.Models;
using WpfAppFastApiUI.Services;
using Size = WpfAppFastApiUI.Models.Size;

namespace WpfAppFastApiUI.View;

public partial class ProductWindow : Window
{
    private CartWindow _cartWindow;
    private int userid;

    public ProductWindow(string categoryName, List<Product> products)
    {
        InitializeComponent();
        this.Closed += WindowClose;

        TitleText.Text = categoryName;
        userid = int.Parse(File.ReadAllText(UserService.filePath));

        _ = InitAsync(products); 
    }

    private async Task InitAsync(List<Product> products)
    {
        try
        {
            var sizeList = await ProductService.GetSizesAsync();
            CreateProducts(products, sizeList);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка загрузки размеров: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void CreateProducts(List<Product> products, List<Size> sizeList)
    {
        foreach (Product product in products)
        {
            var border = new Border
            {
                Width = 200,
                Margin = new Thickness(10),
                Background = Brushes.White,
                CornerRadius = new CornerRadius(5),
                BorderBrush = Brushes.LightGray,
                BorderThickness = new Thickness(1),
                ToolTip = CreateToolTip(product)
            };

            var panel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(10)
            };

            var image = new Image
            {
                Source = new BitmapImage(new Uri(product.img_url, UriKind.RelativeOrAbsolute)),
                Height = 120,
                Stretch = Stretch.Uniform,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var nameText = new TextBlock
            {
                Text = product.name,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(5),
                FontWeight = FontWeights.Bold
            };

            var priceText = new TextBlock
            {
                Text = $"{product.price:C}",
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 5),
                Foreground = Brushes.DarkGreen,
                FontWeight = FontWeights.Bold
            };

            var quantityPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 5, 0, 5)
            };

            var minusButton = new Button
            {
                Content = "-",
                Width = 30,
                Tag = product
            };
            minusButton.Click += QuantityButton_Click;

            var quantityText = new TextBlock
            {
                Text = "1",
                Width = 30,
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            var plusButton = new Button
            {
                Content = "+",
                Width = 30,
                Tag = product
            };
            plusButton.Click += QuantityButton_Click;

            quantityPanel.Children.Add(minusButton);
            quantityPanel.Children.Add(quantityText);
            quantityPanel.Children.Add(plusButton);

            var sizePanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 5, 0, 5)
            };

            foreach (var size in sizeList)
            {
                var radio = new RadioButton
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    Content = size.name,
                    GroupName = $"SizeGroup_{product.id}",
                    Margin = new Thickness(0, 0, 5, 0),
                    Tag = size.name // удобно потом достать
                };
                sizePanel.Children.Add(radio);
            }

            var addButton = new Button
            {
                Content = "Добавить в корзину",
                Margin = new Thickness(0, 5, 0, 0),
                Padding = new Thickness(5),
                Tag = new Tuple<Product, TextBlock, StackPanel>(product, quantityText, sizePanel)
            };
            addButton.Click += AddToCart_Click;

            panel.Children.Add(image);
            panel.Children.Add(nameText);
            panel.Children.Add(priceText);
            panel.Children.Add(quantityPanel);
            panel.Children.Add(sizePanel);
            panel.Children.Add(addButton);
            border.Child = panel;

            ProductPanel.Children.Add(border);
        }
    }

    private ToolTip CreateToolTip(Product product)
    {
        return new ToolTip
        {
            Content = new TextBlock
            {
                Text = product.description,
                TextWrapping = TextWrapping.Wrap,
                MaxWidth = 200
            }
        };
    }

    private void QuantityButton_Click(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        var quantityText = FindQuantityTextBlock(button);

        if (quantityText != null)
        {
            int quantity = int.Parse(quantityText.Text);
            quantity += button.Content.ToString() == "+" ? 1 : (quantity > 1 ? -1 : 0);
            quantityText.Text = quantity.ToString();
        }
    }

    private TextBlock FindQuantityTextBlock(Button button)
    {
        var parent = VisualTreeHelper.GetParent(button) as StackPanel;
        return parent?.Children.OfType<TextBlock>().FirstOrDefault();
    }

    private void AddToCart_Click(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        var tuple = (Tuple<Product, TextBlock, StackPanel>)button.Tag;
        var product = tuple.Item1;
        var quantityText = tuple.Item2;
        var sizePanel = tuple.Item3;

        int quantity = int.Parse(quantityText.Text);
        string selectedSize = sizePanel.Children
            .OfType<RadioButton>()
            .FirstOrDefault(rb => rb.IsChecked == true)?.Tag?.ToString() ?? "Не выбран";

        if (selectedSize == "Не выбран")
        {
            MessageBox.Show("Пожалуйста, выберите размер", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        CartService.AddToCartAsync(userid, product.id, quantity, selectedSize);
        MessageBox.Show($"Добавлено в корзину: {product.name} x{quantity} ({selectedSize})", "Корзина", MessageBoxButton.OK, MessageBoxImage.Information);

        quantityText.Text = "1";
    }

    private void WindowClose(object sender, EventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        this.Hide();
        if (Owner is CategoryWindow categoryWindow)
            categoryWindow.Show();
    }

    private void ButtonBase_OnClick2(object sender, RoutedEventArgs e)
    {
        this.Hide();
        new MainWindow().Show();
    }

    private async void ButtonBase_OnClick3(object sender, RoutedEventArgs e)
    {
        try
        {
            if (_cartWindow == null)
            {
                var cartProducts = await CartService.GetCartProductsAsync(userid);
                _cartWindow = new CartWindow(cartProducts)
                {
                    Owner = this
                };
                _cartWindow.Closed += (_, _) => _cartWindow = null;
            }

            Hide();
            _cartWindow.Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}");
            Show();
        }
    }
}
