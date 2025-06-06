using CarAccountingGibdd.Classes;
using CarAccountingGibdd.Classes.Services;
using CarAccountingGibdd.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Логика взаимодействия для EditApplicationCard.xaml
    /// </summary>
    public partial class EditApplicationDialog : Window
    {
        // Свойства
        public bool Saved { get; private set; } // Флаг сохранения
        public string ApplicationNumber { get; set; } // Свойства для контекста

        // Поля
        private Model.Application _application;

        // Конструктор
        public EditApplicationDialog(Model.Application application)
        {
            InitializeComponent();

            ownerATB.ItemsSource = App.DbContext.Owners;
            vehicleATB.ItemsSource = App.DbContext.Vehicles;

            DataContext = this;
            ApplicationNumber = application.ApplicationId.ToString();

            ownerATB.SelectedItem = application.Owner;
            vehicleATB.SelectedItem = application.Vehicle;

            _application = application;
        }

        // Методы
        private void CreateApplication()
        {
            // Получаем данные для платежа
            Owner owner = (Owner)ownerATB.SelectedItem;
            Vehicle vehicle = (Vehicle)vehicleATB.SelectedItem;

            // Создание экземпляра сервиса для обработки заявки
            ApplicationService applicationService = new(owner, vehicle);

            // Проверка
            bool notError = applicationService.Check(_application);
            if (!notError) return;

            // Подтверждение 
            bool accept = MessageHelper.ConfirmEditApplication();
            if (!accept) return;

            // Формирование 
            applicationService.Edit(_application);

            // Смена флажка о сохранении
            Saved = true;
            Close();
        }
        

        // Обработчики событий
        private void Exit_Click(object sender, RoutedEventArgs e) => MessageHelper.ConfirmExit(this);

        private void Add_Click(object sender, RoutedEventArgs e) => CreateApplication();
    }
}

