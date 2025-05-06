using Avalonia.Media.Imaging;

using System;
using System.ComponentModel;
using System.IO;

namespace AvaloniaProducts;

public class Product : INotifyPropertyChanged
{
    public string ProductName { get; set; } = "";
    public double ProductCost { get; set; }
    public int ProductQuantity { get; set; }

    private string? _imageFileName;
    public string? Image
    {
        get => _imageFileName;
        set
        {
            if (_imageFileName != value)
            {
                _imageFileName = value;
                OnPropertyChanged(nameof(Image));
                OnPropertyChanged(nameof(ProductImage));
            }
        }
    }

    public Bitmap? ProductImage
    {
        get
        {
            if (string.IsNullOrEmpty(Image))
                return null;

            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../Images");
            string path = Path.Combine(folderPath, Image);

            if (File.Exists(path))
            {
                try
                {
                    return new Bitmap(path);
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
