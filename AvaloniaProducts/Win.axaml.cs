using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls.Notifications;
using Avalonia.Media.Imaging;

namespace AvaloniaProducts;

public partial class Win : Window
{
    public List<Product> Products => ProductList.Instance.Products;
    private BasketList basketList = BasketList.Instance;
   


    public Win()
    {
        InitializeComponent();
        ProductListBox.ItemsSource = Products;
    }

    private void AddToBasket_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is Product product)
        {
            if (product.ProductQuantity == 0)
            {
                new Window1("Такого товара больше нет в наличии.").ShowDialog(this);
            }

            if (product.ProductQuantity > 0)
            {
                basketList.AddToBasket(product.ProductName, 1);
                ProductListBox.ItemsSource = null;
                ProductListBox.ItemsSource = Products;
            }
        }
    }

    private void Del_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is Product deleteProduct)
        {
            ProductList.Instance.RemoveProduct(deleteProduct);
            BasketList.Instance.RemoveFromBasket(deleteProduct);

            ProductListBox.ItemsSource = null;
            ProductListBox.ItemsSource = Products;
        }
        if (ProductList.Instance.Products.Count == 0)
        {
            ProductListBox.ItemsSource = null;
            ProductListBox.ItemsSource = new List<string> { "Список продуктов пуст." };
        }
    }

    private void Edit_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is Product productToEdit)
        {
            WinEditProduct editWindow = new WinEditProduct(productToEdit);
            editWindow.Show();
            Close();
        }
    }

    private void Basket_Click(object? sender, RoutedEventArgs e)
    {
        var productsInStore = ProductList.Instance.Products.Select(p => p.ProductName).ToHashSet();
        basketList.Basket.RemoveAll(p => !productsInStore.Contains(p.ProductName));
        if (basketList.Basket.Count == 0)
        {
            new Window1("Ваша корзина пуста.").ShowDialog(this);
        }
        else
        {
            BasketWindow basket = new BasketWindow();
            basket.Show();
            Close();
        }
    }

    private void Return_Click(object? sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        Close();
    }
}