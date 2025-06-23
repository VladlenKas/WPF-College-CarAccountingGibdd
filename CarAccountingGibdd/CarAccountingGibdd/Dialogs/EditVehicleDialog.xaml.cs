using CarAccountingGibdd.Classes;
using CarAccountingGibdd.Classes.Services;
using CarAccountingGibdd.Model;
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
    /// Логика взаимодействия для EditVehicleDialog.xaml
    /// </summary>
    public partial class EditVehicleDialog : Window
    {
        // Свойства
        public bool Saved { get; private set; } // Флаг сохранения

        // Поля
        private Vehicle _vehicle;
        private List<PhotosVehicle> _photosVehicles;

        // Конструктор
        public EditVehicleDialog(Vehicle vehicle)
        {
            InitializeComponent();

            typeCB.ItemsSource = App.DbContext.VehicleTypes.ToList();

            vinTB.Text = vehicle.Vin;
            licensePlateTB.Text = vehicle.LicensePlate;
            markaTB.Text = vehicle.Brand;
            modelTB.Text = vehicle.Model;
            yearTB.Text = vehicle.Year.ToString();
            colorTB.Text = vehicle.Color;
            typeCB.SelectedItem = vehicle.VehicleType;
            _photosVehicles = new List<PhotosVehicle>(vehicle.PhotosVehicles.ToList());

            if (_photosVehicles.Count > 0)
            {
                addImagesBTN.Content = $"Фото выбрано: {_photosVehicles.Count}";
            }
            else
            {
                addImagesBTN.Content = "Добавить фото";
            }
            
            // Если у владельца гос. номер выдан по свидетельству, то его поменять нельзя
            bool hasCertificate = vehicle.Applications?.Any(a => a.Certificates?.Count > 0) == true;
            if (hasCertificate)
            {
                licensePlateTB.IsEnabled = false;
            }

            _vehicle = vehicle;
        }

        private void Edit()
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
            bool notError = service.Check(_vehicle);
            if (!notError) return;

            // Подтверждение
            bool accept = MessageHelper.ConfirmEdit();
            if (!accept) return;

            // Формирование
            service.Update(_vehicle);

            // Закрываем и сменяем флажок 
            Saved = true;
            Close();
        }

        private void AddImages()
        {
            this.Hide();

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

            this.ShowDialog();
        }

        // Обработчики событий
        private void Exit_Click(object sender, RoutedEventArgs e) => MessageHelper.ConfirmExit(this);

        private void Edit_Click(object sender, RoutedEventArgs e) => Edit();

        private void AddImages_Click(object sender, RoutedEventArgs e) => AddImages();
    }
}
