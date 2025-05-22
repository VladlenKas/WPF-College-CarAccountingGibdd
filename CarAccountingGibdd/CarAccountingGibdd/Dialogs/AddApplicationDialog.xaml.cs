using CarAccountingGibdd.Classes;
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
    /// Логика взаимодействия для AddApplicationDialog.xaml
    /// </summary>
    public partial class AddApplicationDialog : Window, INotifyPropertyChanged
    {
        // Поля и свойства
        public bool Saved { get; private set; }

        // Конструктор
        public AddApplicationDialog()
        {
            InitializeComponent();
            ownerATB.ItemsSource = App.DbContext.Owners.ToList();
            vehicleATB.ItemsSource = null;
        }

        // Методы
        private void CreateApplication(Owner owner, Vehicle vehicle)
        {
           /* bool notError = Limitators.RateLimitator(null, name, cost, description);
            if (!notError) return;

            bool accept = MessageHelper.ConfirmSaveApplication();
            if (!accept) return;

            RateService.AddRate(name, cost, description);
            Saved = true;
            Close();*/
        }

        // Обработчики событий
        private void Exit_Click(object sender, RoutedEventArgs e) => MessageHelper.ConfirmExit(this);

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Owner owner = (Owner)ownerATB.SelectedItem;
            Vehicle vehicle = (Vehicle)vehicleATB.SelectedItem;

            CreateApplication(owner, vehicle);
        }

        // Реализация интерфейса
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
