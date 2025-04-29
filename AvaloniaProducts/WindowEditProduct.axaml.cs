using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Interactivity;
using System.Linq;

namespace AvaloniaProducts;

public partial class WinEditProduct : Window
{
    private Product _product;
    private ProductList productList = ProductList.Instance;

    public WinEditProduct(Product product)
    {
        InitializeComponent();
        _product = product;

        ProductNameTextBox.Text = _product.ProductName;
        ProductCostTextBox.Text = _product.ProductCost.ToString();
        ProductQuantityTextBox.Text = _product.ProductQuantity.ToString();
    }

    public void SaveChanges_Click(object? sender, RoutedEventArgs e)
    {
        var nameTextBox = this.FindControl<TextBox>("ProductNameTextBox");
        var costTextBox = this.FindControl<TextBox>("ProductCostTextBox");
        var quantityTextBox = this.FindControl<TextBox>("ProductQuantityTextBox");

        if (nameTextBox == null || string.IsNullOrWhiteSpace(nameTextBox.Text))
        {
            ShowNameErrorMessage();
            return;
        }

        foreach (var product in productList.Products.Where(p => p != _product))
        {
            if (product.ProductName == nameTextBox.Text)
            {
                ShowDoubleErrorMessage();
                return;
            }
        }

        if (!double.TryParse(costTextBox.Text, out double newCost) || newCost <= 0)
        {
            ShowCostErrorMessage();
            return;
        }

        if (!int.TryParse(quantityTextBox.Text, out int newQuantity) || newQuantity <= 0)
        {
            ShowQuantityErrorMessage();
            return;
        }

        string oldName = _product.ProductName;

        _product.ProductName = nameTextBox.Text;
        _product.ProductCost = newCost;
        _product.ProductQuantity = newQuantity;

        var productsInBasket = BasketList.Instance.Basket;
        foreach (var productInBasket in productsInBasket)
        {
            if (productInBasket.ProductName == oldName)
            {
                productInBasket.ProductName = _product.ProductName;
                productInBasket.ProductCost = _product.ProductCost * productInBasket.ProductQuantity;
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

    private void ShowCostErrorMessage()
    {
        var notificationManager = new WindowNotificationManager(this)
        {
            Position = NotificationPosition.BottomCenter
        };
        notificationManager.Show(new Notification("Ошибка", "Некорректная цена товара.", NotificationType.Error));
    }
    private void ShowQuantityErrorMessage()
    {
        var notificationManager = new WindowNotificationManager(this)
        {
            Position = NotificationPosition.BottomCenter
        };
        notificationManager.Show(new Notification("Ошибка", "Некорректное количество товара.", NotificationType.Error));
    }
    private void ShowNameErrorMessage()
    {
        var notificationManager = new WindowNotificationManager(this)
        {
            Position = NotificationPosition.BottomCenter
        };
        notificationManager.Show(new Notification("Ошибка", "Товар без названия.", NotificationType.Error));
    }
    private void ShowDoubleErrorMessage()
    {
        var notificationManager = new WindowNotificationManager(this)
        {
            Position = NotificationPosition.BottomCenter
        };
        notificationManager.Show(new Notification("Ошибка", "Такой продукт уже есть.", NotificationType.Error));
    }
}