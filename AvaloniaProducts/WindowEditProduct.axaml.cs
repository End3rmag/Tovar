
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using System;
using System.IO;
using System.Linq;
using System.Runtime;

namespace AvaloniaProducts;

public partial class WinEditProduct : Window
{
    private Product _product;
    private ProductList productList = ProductList.Instance;
    private string? newPhoto = null;

    public WinEditProduct()
    {
        InitializeComponent();
    }

    public WinEditProduct(Product product)
    {
        InitializeComponent();
        _product = product;
        
        ProductNameTextBox.Text = _product.ProductName;
        ProductCostTextBox.Text = _product.ProductCost.ToString();
        ProductQuantityTextBox.Text = _product.ProductQuantity.ToString();

        string img = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../Images", _product.Image);
        ProductsImage.Source = new Bitmap(img);
    }

    private void SaveChanges_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var nameTextBox = this.FindControl<TextBox>("ProductNameTextBox");
        var costTextBox = this.FindControl<TextBox>("ProductCostTextBox");
        var quantityTextBox = this.FindControl<TextBox>("ProductQuantityTextBox");

        if (nameTextBox == null || string.IsNullOrWhiteSpace(nameTextBox.Text))
        {
            new Window1("Товар без названия.").ShowDialog(this);
            return;
        }

        foreach (var product in productList.Products.Where(p => p != _product))
        {
            if (product.ProductName == nameTextBox.Text)
            {
                new Window1("Такой продукт уже есть.").ShowDialog(this);
                return;
            }
        }

        if (!double.TryParse(costTextBox.Text, out double newCost) || newCost <= 0)
        {
            new Window1("Некорректная цена товара.").ShowDialog(this);
            return;
        }

        if (!int.TryParse(quantityTextBox.Text, out int newQuantity) || newQuantity <= 0)
        {
            new Window1("Некорректное количество товара.").ShowDialog(this);
            return;
        }

        string oldName = _product.ProductName;

        _product.ProductName = nameTextBox.Text;
        _product.ProductCost = newCost;
        _product.ProductQuantity = newQuantity;
        _product.Image = newPhoto ?? _product.Image;
         


        var productsInBasket = BasketList.Instance.Basket;
        foreach (var productInBasket in productsInBasket)
        {
            if (productInBasket.ProductName == oldName)
            {
                productInBasket.ProductName = _product.ProductName;
                productInBasket.ProductCost = _product.ProductCost * productInBasket.ProductQuantity;
                productInBasket.Image = newPhoto ?? _product.Image;

            }
        }

        Win win = new Win();
        win.Show();
        this.Close();
    }

    private void Return_Click(object? sender, RoutedEventArgs e)
    {
        Win win = new Win();
        win.Show();
        this.Close();
    }
    private void UpdateProductImage()
    {   
        var imageControl = this.FindControl<Image>("ProductsImage");
        if (imageControl != null)
        {
            if (!string.IsNullOrEmpty(_product.Image) && File.Exists(_product.Image))
            {
                
                try
                {
                    var bitmap = new Bitmap(_product.Image);
                    imageControl.Source = bitmap;
                }
                catch (Exception ex)
                {
                    new Window1("Ошибка при загрузке изображения.").ShowDialog(this);
                    imageControl.Source = null;
                }
            }
            else
            {
                
                imageControl.Source = null;
            }
        }
    }


    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _product.Image = null;
        UpdateProductImage();
    }

    private async void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var ofd = new OpenFileDialog();
        ofd.Filters.Add(new FileDialogFilter() { Name = "Image Files", Extensions = { "png", "jpg", "jpeg", "bmp" } });
        ofd.AllowMultiple = false;

        var result = await ofd.ShowAsync(this);
        if (result != null && result.Length > 0)
        {
            string selectedFile = result[0];

            _product.Image = selectedFile;
        }
        UpdateProductImage();
    }
    
}

