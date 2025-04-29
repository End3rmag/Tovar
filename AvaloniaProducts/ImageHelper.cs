using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace AvaloniaProducts
{
    public static class ImageHelper
    {
        public static Bitmap LoadFromResources(Uri resourceUri)
        {
            return new Bitmap(AssetLoader.Open(resourceUri));
        }

        public static async Task<Bitmap?> LoadFromWeb(Uri webUri)
        {
            using var httpClient = new HttpClient();
            try
            {
                var response = await httpClient.GetAsync(webUri);
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsByteArrayAsync();
                return new Bitmap(new MemoryStream(data));

            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"An error occurred while downloading image '{webUri}' : {ex.Message}");
                return null;
            }
        }
    }
}

// что не так, почему возникает эта ошибка? пути расположения файлов указано правильно
// обязательно ли добавлять Bitmap в класс продуктов? без этого вылезает ошибка с биндингами, что в авалонияПродукт нет такого. можно ли как-то это исправить, не добавляя его в класс продукт?