using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using System;
using System.IO;

namespace AvaloniaProducts;

public partial class MainWindow : Window
{
    private ProductList productList = ProductList.Instance;
    private Win _list = new Win();
    private string? _selectedImageFileName;
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void BtnAddPicture_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {

        var openFileDialog = new OpenFileDialog();
        openFileDialog.Filters.Add(new FileDialogFilter()
        {
            Name = "Images",
            Extensions = { "png", "jpg", "jpeg" }
        });

        var result = await openFileDialog.ShowAsync(this);
        if (result != null && result.Length > 0)
        {
            string selectedFile = result[0];
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../Images");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            _selectedImageFileName = $"photo_{Guid.NewGuid()}.jpg";
            string destPath = Path.Combine(folderPath, _selectedImageFileName);
            File.Copy(selectedFile, destPath, true);
        }
    }

    private void BtnAddProduct_Click(object? sender, RoutedEventArgs e)
    {
        string enteredProductName = TextBoxName.Text;
        foreach (var product in productList.Products)
        {
            if (product.ProductName == enteredProductName)
            {
                new Window1("Такой продукт уже есть.").ShowDialog(this);
                return;
            }
        }
        if (string.IsNullOrWhiteSpace(enteredProductName))
        {
            new Window1("Товар без названия.").ShowDialog(this);
            return;
        }
        if (!double.TryParse(TextBoxCost.Text, out double enteredCostOfProduct) || enteredCostOfProduct <= 0)
        {
            new Window1("Некорректная цена продукта.").ShowDialog(this);
            return;
        }
        if (!int.TryParse(TextBoxQuantity.Text, out int enteredQuantityOfProduct) || enteredQuantityOfProduct <= 0)
        {
            new Window1("Некорректное количество продукта.").ShowDialog(this);
            return;
        }

        string photoFileNameToUse = _selectedImageFileName ?? "";

        productList.AddProduct(enteredProductName, enteredCostOfProduct, enteredQuantityOfProduct, _selectedImageFileName ?? "");

        TextBoxName.Text = "";
        TextBoxCost.Text = "";
        TextBoxQuantity.Text = "";
        _selectedImageFileName = null;
    }
    

    private void BtnShowAllProducts_Click(object? sender, RoutedEventArgs e)
    {
        if (productList.Products.Count > 0)
        {
            Win Win = new Win();
            Win.Show();
            Close();
        }
        else
        {
            new Window1("Список продуктов пуст.").ShowDialog(this);
        }
    }

   
}