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

            _vehicle = vehicle;
        }

        private void UpdateVehicle()
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
            VehicleService service = new VehicleService(vin, brand, model, year, color, licensePlate, type);

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

        // Обработчики событий
        private void Exit_Click(object sender, RoutedEventArgs e) => MessageHelper.ConfirmExit(this);

        private void Add_Click(object sender, RoutedEventArgs e) => UpdateVehicle();
    }
}
