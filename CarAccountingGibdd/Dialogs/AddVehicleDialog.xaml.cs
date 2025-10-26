using CarAccountingGibdd.Classes;
using CarAccountingGibdd.Classes.Services;
using CarAccountingGibdd.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    /// Логика взаимодействия для AddVehicleDialog.xaml
    /// </summary>
    public partial class AddVehicleDialog : Window
    {
        // Свойства
        public bool Saved { get; private set; } // Флаг сохранения

        // Поля
        private List<PhotosVehicle> _photosVehicles;

        // Конструктор
        public AddVehicleDialog()
        {
            InitializeComponent();

            typeCB.ItemsSource = App.DbContext.VehicleTypes.ToList();
            _photosVehicles = new();
        }

        // Методы
        private void Add()
        {
            // Получаем данные для добавления
            string vin = vinTB.Text;
            string licensePlate = licensePlateTB.Text;
            string brand = markaTB.Text;
            string model = modelTB.Text;
            string year = yearTB.Text;
            string color = colorTB.Text;
            VehicleType? type = (VehicleType?)typeCB.SelectedItem;

            // Создаем экземпляр сервиса
            VehicleService service = new VehicleService(vin, brand, model, year, color, licensePlate, type, _photosVehicles);

            // Проверка
            bool notError = service.Check();
            if (!notError) return;

            // Подтверждение
            bool accept = MessageHelper.ConfirmSave();
            if (!accept) return;

            // Формирование
            service.Add();

            // Закрываем и сменяем флажок 
            Saved = true;
            Close();
        }

        private void AddImages()
        {
            AddImageForVehicleDialog dialog = new AddImageForVehicleDialog(_photosVehicles);
            dialog.ShowDialog();

            bool saved = dialog.Saved;
            if (saved)
            {
                int countImages = dialog.CountImages;
                if (countImages > 0)
                {
                    addImagesBTN.Content = $"Фото выбрано: {countImages}";
                }
                else
                {
                    addImagesBTN.Content = "Добавить фото";
                }

                _photosVehicles = dialog.PhotosVehicles;
            }
        }

        // Обработчики событий
        private void Exit_Click(object sender, RoutedEventArgs e) => MessageHelper.ConfirmExit(this);

        private void Add_Click(object sender, RoutedEventArgs e) => Add();

        private void AddImages_Click(object sender, RoutedEventArgs e) => AddImages();
    }
}
