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
        public bool Saved { get; private set; } // Флаг сохранения
        public string ChangeForPayment { get; set; } // Свойства для контекста

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
        private void CreateApplication()
        {
            // Получаем данные для платежа
            Owner owner = (Owner)ownerATB.SelectedItem;
            Vehicle vehicle = (Vehicle)vehicleATB.SelectedItem;
            int index = paymentCB.SelectedIndex;
            int change = AmountChanged();
            string? bank = index == 1 ? string.Empty : bankPaymentCB.SelectedValue?.ToString();

            // Создание экземпляра сервиса для обработки заявки
            ApplicationService applicationService = new(owner, vehicle, index, change, bank);

            // Проверка
            bool notError = applicationService.Check();
            if (!notError) return;

            // Подтверждение 
            bool accept = MessageHelper.ConfirmSaveApplication();
            if (!accept) return;

            // Формирование 
            applicationService.Create();

            // Смена флажка о сохранении
            Saved = true;
            Close();
        }

        private int AmountChanged()
        {
            int index = paymentCB.SelectedIndex;
            int change = 0;

            if (index == 1)
            {
                string text = cashPaymentTB.Text;
                int amount = TypeHelper.IntParse(text);
                change = amount - 400;
            }

            ChangeForPayment = $"{change} р.";
            OnPropertyChanged(nameof(ChangeForPayment));

            return change;
        }

        // Обработчики событий
        private void Exit_Click(object sender, RoutedEventArgs e) => MessageHelper.ConfirmExit(this);

        private void Add_Click(object sender, RoutedEventArgs e) => CreateApplication();

        private void PaymentCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = paymentCB.SelectedIndex;

            if (index == 0)
            {
                bankPaymentCB.Visibility = Visibility.Visible;
                cashPaymentTB.Visibility = Visibility.Collapsed;
            }

            if (index == 1)
            {
                bankPaymentCB.Visibility = Visibility.Collapsed;
                cashPaymentTB.Visibility = Visibility.Visible;
            }

            AmountChanged();
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
