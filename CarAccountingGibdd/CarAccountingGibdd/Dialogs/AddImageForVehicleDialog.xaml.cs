using CarAccountingGibdd.Classes;
using CarAccountingGibdd.Controls;
using CarAccountingGibdd.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CarAccountingGibdd.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для AddImageForVehicleDialog.xaml
    /// </summary>
    public partial class AddImageForVehicleDialog : Window
    {
        // Свойства
        public bool Saved { get; private set; } // Флаг сохранения
        public List<PhotosVehicle> PhotosVehicles { get; set; }
        public int CountImages => PhotosVehicles.Count;

        // Поля
        private List<PhotosVehicle> _photosVehicles;

        // Конструктор
        public AddImageForVehicleDialog(List<PhotosVehicle> photosVehicles)
        {
            InitializeComponent();

            _photosVehicles = new List<PhotosVehicle>(photosVehicles); // Создаем копию
            PhotosVehicles = photosVehicles;

            UpdateIC();
        }

        // Методы
        private void UpdateIC()
        {
            cardsIC.Items.Clear();
            foreach (var photo in _photosVehicles)
            {
                var card = new ImageMiniatureCard(photo, this);
                card.ImageDeleteEvent += ImageDelete;
                cardsIC.Items.Add(card);
            }
        }

        private void AddImages()
        {
            // Проверка на количество фото
            if (_photosVehicles.Count >= 10)
            {
                MessageHelper.MessageUniversal("Максимальное количество фотографий — 10 штук!");
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Title = "Выберите фотографии",
                Filter = "Изображения (*.jpg;*.jpeg;*.png;*.bmp;*.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Все файлы (*.*)|*.*",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (var filePath in openFileDialog.FileNames)
                {
                    // Проверка на количество фото
                    if (_photosVehicles.Count >= 10)
                    {
                        break;
                    }

                    // Преобразуем путь к файлу в BitmapImage
                    var bitmap = TypeHelper.PathToBitmapImage(filePath);

                    // Преобразуем BitmapImage в byte[]
                    PhotosVehicle photo = new PhotosVehicle();
                    photo.Photo = TypeHelper.ImageToByteArray(bitmap);

                    // Добавляем ко всем фотографиям
                    _photosVehicles.Add(photo);

                    UpdateIC();
                }
            }
        }

        // Обработчики событий
        private void ImageDelete(object sender, PhotosVehicleEventArgs e)
        {
            PhotosVehicle photo = sender as PhotosVehicle;
            _photosVehicles.Remove(photo);
            UpdateIC();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MessageHelper.ConfirmExit(this);
        }

        private void Add_Click(object sender, RoutedEventArgs e) 
        {
            PhotosVehicles = _photosVehicles;
            Saved = true; 
            Close(); 
        }

        private void AddImages_Click(object sender, RoutedEventArgs e) => AddImages();
    }
}
