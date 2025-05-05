using Avalonia.Media.Imaging;
using System;

namespace AvaloniaProducts;

public class Product
{
    public string ProductName { get; set; }
    public double ProductCost { get; set; }
    public int ProductQuantity { get; set; }
    
    public string Image { get; set; }

    public Bitmap? ProductImage
    {
        get
        {
            if (Image != "" && Image != " " && Image != null)
            {
                return new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "AvaloniaProducts/Images/" + Image);
            }
            else
            {
                return new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "../../../Images/ip.jpg");
            }
        }
        set { }
    }
}