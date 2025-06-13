using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CarAccountingGibdd.Classes
{
    public static class TypeHelper
    {
        // Преобразовывает текст в дату либо возвращает мин. значение
        public static DateOnly DateOnlyParse(string str)
        {
            try
            {
                DateOnly number = DateOnly.Parse(str);
                return number;
            }
            catch
            {
                return DateOnly.MinValue;
            }
        }

        // Преобразовывает текст в decimal либо возвращает 0
        public static decimal DecemalParse(string str)
        {
            try
            {
                decimal number = Decimal.Parse(str);
                return number;
            }
            catch
            {
                return 0;
            }
        }

        // Преобразовывает текст в строку либо возвращает 0
        public static int IntParse(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }
            else
            {
                int number = Convert.ToInt32(str);
                return number;
            }
        }

        // Преобразовывает текст в short либо возвращает -1
        public static short ShortParse(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return -1;
            }
            else
            {
                int year = Convert.ToInt32(str);
                return (short)year;
            }
        }

        // Конвертер из byte[] в BitmapImage
        public static BitmapImage GetBitmapImage(byte[] photo)
        {
            if (photo == null || photo.Length == 0)
                return null;

            using (var ms = new MemoryStream(photo))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                image.Freeze();
                return image;
            }
        }

        // Конвертер из BitmapImage в byte[]
        public static byte[] ImageToByteArray(BitmapSource image)
        {
            if (image == null)
                return Array.Empty<byte>();

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            using (var ms = new MemoryStream())
            {
                encoder.Save(ms);
                return ms.ToArray();
            }
        }

        // Конвертер из пути в BitmapImage
        public static BitmapImage PathToBitmapImage(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return null;

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(filePath, UriKind.Absolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            bitmap.Freeze(); // Делаем изображение потокобезопасным и неизменяемым

            return bitmap;
        }
    }
}
