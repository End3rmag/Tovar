using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaloniaProducts;

public partial class Window1 : Window
{
    public Window1()
    {
        InitializeComponent();
    }

    public Window1(string a) 
    {
        InitializeComponent();
        Eror.Text = a;
    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close();
    }
}