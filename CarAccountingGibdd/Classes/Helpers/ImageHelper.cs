using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace CarAccountingGibdd.Classes
{
    public static class ImageHelper
    {
        // Путь к изображению-заглушке по умолчанию
        private static readonly string DefaultImagePath =
            "pack://application:,,,/CarAccountingGibdd;component/Images/NoImageVehicle.png";

        /// <summary>
        /// Открывает диалог выбора изображения и возвращает его данные
        /// </summary>
        /// <param name="imageBytes">Массив байтов выбранного изображения (null при ошибке/отмене)</param>
        /// <param name="imageSource">ImageSource выбранного изображения (заглушка при ошибке/отмене)</param>
        /// <returns>True - изображение выбрано успешно, False - отмена выбора или ошибка</returns>
        public static bool TrySelectImage(out byte[] imageBytes, out ImageSource imageSource)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png, *.webp)|*.jpg;*.jpeg;*.png;*.webp",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    // Чтение файла и преобразование в BitmapImage
                    imageBytes = File.ReadAllBytes(dialog.FileName);
                    imageSource = new BitmapImage(new Uri(dialog.FileName));
                    return true;
                }
                catch
                {
                    // В случае ошибки возвращаем заглушку
                    imageBytes = null;
                    imageSource = GetDefaultImage();
                    return false;
                }
            }

            // Пользователь отменил выбор
            imageBytes = null;
            imageSource = GetDefaultImage();
            return false;
        }

        /// <summary>
        /// Возвращает изображение-заглушку по умолчанию
        /// </summary>
        public static ImageSource GetDefaultImage()
        {
            return new BitmapImage(new Uri(DefaultImagePath, UriKind.Absolute));
        }

        /// <summary>
        /// Конвертирует ImageSource в массив байтов (формат JPEG)
        /// </summary>
        /// <param name="imageSource">Исходное изображение</param>
        /// <returns>Массив байтов или null для некорректных форматов</returns>
        public static byte[] ImageSourceToBytes(ImageSource imageSource)
        {
            if (imageSource is BitmapSource bitmapSource)
            {
                // Используем JPEG-энкодер для преобразования
                var encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    return stream.ToArray();
                }
            }
            return null;
        }

        /// <summary>
        /// Сравнивает два изображения по их байтовому представлению
        /// </summary>
        /// <param name="image1">Первое изображение</param>
        /// <param name="image2">Второе изображение</param>
        /// <returns>
        /// True - изображения идентичны, 
        /// False - разные изображения или ошибка сравнения
        /// </returns>
        public static bool CompareImages(byte[] image1, byte[] image2)
        {
            if (image1 == null || image2 == null)
                return image1 == image2; // Оба null = считаем одинаковыми

            return image1.SequenceEqual(image2); // Побайтовое сравнение
        }
    }
}
