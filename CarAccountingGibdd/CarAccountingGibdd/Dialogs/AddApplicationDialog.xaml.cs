﻿using CarAccountingGibdd.Classes;
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
        // Свойства
        public bool Saved { get; private set; } // Флаг сохранения
        public string ChangeForPayment { get; set; } // Свойства для контекста

        // Поля
        private Employee _operator;

        // Конструктор
        public AddApplicationDialog(Employee @operator)
        {
            InitializeComponent();

            var excludedStatusIds = new[] { 1, 2, 3, 4 };
            vehicleATB.ItemsSource = App.DbContext.Vehicles
                .Where(v =>
                    !v.Applications.Any(a => a.Certificates.Any(c => c.IsActive == 1)) &&
                    !v.Applications.Any(a => excludedStatusIds.Contains(a.ApplicationStatusId))
                );

            ownerATB.ItemsSource = App.DbContext.Owners;
            paymentCB.ItemsSource = new[] { "Безналичный расчет", "Наличный расчет" };

            DataContext = this;
            ChangeForPayment = "0 р.";

            _operator = @operator;
        }

        // Методы
        private void Create()
        {
            // Получаем данные для платежа
            Owner owner = (Owner)ownerATB.SelectedItem;
            Vehicle vehicle = (Vehicle)vehicleATB.SelectedItem;
            int index = paymentCB.SelectedIndex;
            int change = AmountChanged();
            Card? card = null;

            if (index == 0)
            {
                string? number = cardNumberTB.CardNumber;
                int? month = cardDateTB.CardMonth;
                int? year = cardDateTB.CardYear;
                int code = TypeHelper.IntParse(cardCodeTB.Text);

                card = new Card()
                {
                    Number = number,
                    Month = month,
                    Year = year,
                    Code = code
                };
            }

            // Создание экземпляра сервиса для обработки заявки
            ApplicationService applicationService = new(owner, vehicle, index, change, card);

            // Проверка
            bool notError = applicationService.Check();
            if (!notError) return;

            // Подтверждение 
            bool accept = MessageHelper.ConfirmSaveApplication();
            if (!accept) return;

            // Закрываем
            Hide();

            // Показываем окно загрузки оплаты
            PaymentMessageDialog dialog = new PaymentMessageDialog();
            ComponentsHelper.ShowDialogWindowDark(dialog);

            // Если произошла отмена
            if (!dialog.Saved)
            {
                ComponentsHelper.ShowDialogWindowDark(this);
                return;
            }

            // Формирование 
            applicationService.Create(_operator);

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

        private void Add_Click(object sender, RoutedEventArgs e) => Create();

        private void PaymentCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = paymentCB.SelectedIndex;

            if (index == 0)
            {
                cardPaymentDP.Visibility = Visibility.Visible;
                cashPaymentTB.Visibility = Visibility.Collapsed;
            }

            if (index == 1)
            {
                cardPaymentDP.Visibility = Visibility.Collapsed;
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
