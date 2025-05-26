using CarAccountingGibdd.Classes;
using CarAccountingGibdd.Classes.Services;
using CarAccountingGibdd.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private ApplicationService ApplicationService => new ApplicationService();
        public bool Saved { get; private set; }

        // Свойства для контекста
        public string ChangeForPayment { get; set; }

        // Конструктор
        public AddApplicationDialog()
        {
            InitializeComponent();
            ownerATB.ItemsSource = App.DbContext.Owners;
            vehicleATB.ItemsSource = App.DbContext.Vehicles;
            paymentCB.ItemsSource = new[] { "Безналичный", "Наличный" };
            bankPaymentCB.ItemsSource = new[] { "Тинькофф", "Сбербанк", "Альфа-банк", "ВТБ", "Газпром" };

            DataContext = this;
            ChangeForPayment = "0 р.";
        }

        // Методы
        private void CreateApplication(Owner owner, Vehicle vehicle)
        {
            // Получаем данные для платежа
            int index = paymentCB.SelectedIndex;
            string bank = bankPaymentCB.SelectedValue?.ToString();
            int change = CalculateChange();

            // Проверка
            bool notError = ApplicationService.Check(owner, vehicle, index, change, bank);
            if (!notError) return;

            // Подтверждение 
            bool accept = MessageHelper.ConfirmSaveApplication();
            if (!accept) return;

            // Формирование 
            ApplicationService.CreateApplication(owner, vehicle, index, change, bank);
            Saved = true;
            Close();
        }

        private void AmountChanged()
        {
            int change = CalculateChange();
             
            ChangeForPayment = $"{change} р.";
            OnPropertyChanged(nameof(ChangeForPayment));
        }

        private int CalculateChange()
        {
            string text = cashPaymentTB.Text;
            int amount = TypeHelper.IntParse(text);
            int change = amount - 400;

            return change;
        }
        // Обработчики событий
        private void Exit_Click(object sender, RoutedEventArgs e) => MessageHelper.ConfirmExit(this);

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Owner owner = (Owner)ownerATB.SelectedItem;
            Vehicle vehicle = (Vehicle)vehicleATB.SelectedItem;

            CreateApplication(owner, vehicle);
        }

        private void PaymentCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = paymentCB.SelectedIndex;
            if (index == 0)
            {
                bankPaymentCB.Visibility = Visibility.Visible;
                cashPaymentTB.Visibility = Visibility.Collapsed;
                // Меняем цену и обновляем видимость
                ChangeForPayment = "0";
                OnPropertyChanged(nameof(ChangeForPayment)); 
            }
            else if (index == 1)
            {
                bankPaymentCB.Visibility = Visibility.Collapsed;
                cashPaymentTB.Visibility = Visibility.Visible;
                // Меняем цену и обновляем видимость
                AmountChanged();
            }
            else
            {
                MessageBox.Show("Неизвестная ошибка.",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void CashPaymentTB_TextChanged(object sender, TextChangedEventArgs e) => AmountChanged();
        
        // Реализация интерфейса
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
