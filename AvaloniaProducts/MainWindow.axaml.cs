using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Interactivity;

namespace AvaloniaProducts;

public partial class MainWindow : Window
{
    private ProductList productList = ProductList.Instance;
    public MainWindow()
    {
        InitializeComponent();
    }

    private void BtnAddProduct_Click(object? sender, RoutedEventArgs e)
    {
        string enteredProductName = TextBoxName.Text;

        foreach(var product in productList.Products)
        {
            if (product.ProductName == enteredProductName)
            {
                new Window1("Такой продукт уже есть.").ShowDialog(this);
                return;
                
            }
        }
        if (enteredProductName == null || enteredProductName == "")
        {
            new Window1("Товар без названия.").ShowDialog(this);
            return;
        }
        if (!double.TryParse(TextBoxCost.Text, out double enteredCostOfProduct) || enteredCostOfProduct <= 0)
        {
            new Window1("Некорректная цена продукта.").ShowDialog(this);
            return;
        }
        if(!int.TryParse(TextBoxQuantity.Text, out int enteredQuantityOfProduct) || enteredQuantityOfProduct <= 0)
        {
            new Window1("Некорректное количество продукта.").ShowDialog(this);
            return;
        }
        else
        {
            productList.AddProduct(enteredProductName, enteredCostOfProduct, enteredQuantityOfProduct);

            TextBoxName.Text = "";
            TextBoxCost.Text = "";
            TextBoxQuantity.Text = "";
        }
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