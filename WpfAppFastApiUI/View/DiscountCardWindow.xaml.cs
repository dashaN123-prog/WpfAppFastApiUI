using System;
using System.IO;
using System.Windows;
using WpfAppFastApiUI.Services;

namespace WpfAppFastApiUI.View
{
    public partial class DiscountCardWindow : Window
    {
        public DiscountCardWindow()
        {
            InitializeComponent();
        }
        

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}