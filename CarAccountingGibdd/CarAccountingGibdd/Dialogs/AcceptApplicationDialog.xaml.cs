using CarAccountingGibdd.Classes.Services;
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
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CarAccountingGibdd.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для AcceptApplicationDialog.xaml
    /// </summary>
    public partial class AcceptApplicationDialog : Window
    {
        // Поля 
        private DateTime? _selectedDate;
        private DateTime? _selectedTime;

        private Model.Application _application;
        private Employee _inspector;

        // Свойства
        public bool Saved { get; private set; } // Флаг сохранения

        // Конструктор
        public AcceptApplicationDialog(Model.Application application, Employee inspector)
        {
            InitializeComponent();

            _application = application;
            _inspector = inspector;

            FillAvailableDates();
        }

        // Методы
        private void FillAvailableDates()
        {
            List<TimeSpan> timeSlots = new List<TimeSpan>
            {
                new TimeSpan(10, 0, 0),
                new TimeSpan(12, 0, 0),
                new TimeSpan(14, 0, 0),
                new TimeSpan(16, 0, 0)
            };

            var allDates = Enumerable.Range(0, 30)
                .Select(offset => DateTime.Today.AddDays(offset))
                .ToList();

            var availableDates = new List<DateTime>();

            foreach (var date in allDates)
            {
                var bookedSlots = App.DbContext.Inspections
                    .Where(i => i.InspectorId == _inspector.EmployeeId)
                    .Where(r => r.DatetimePlanned.Date == date.Date)
                    .Select(r => r.DatetimePlanned.TimeOfDay)
                    .ToList();

                bool hasFreeSlot = timeSlots.Any(slot =>
                    date.Date.Add(slot) > DateTime.Now && // Исключаем прошедшие слоты
                    !bookedSlots.Contains(slot)
                );

                if (hasFreeSlot)
                    availableDates.Add(date);
            }

            dateCB.ItemsSource = availableDates;
        }

        private void VisibleTimeComboBox()
        {
            // Очищаем комбобокс
            timeCB.ItemsSource = null;

            if (dateCB.SelectedIndex != -1)
            {
                // Получаем выбранную дату
                _selectedDate = (DateTime)dateCB.SelectedItem;

                // Получаем все заплнированные инспекции в этот день
                List<DateTime> plannedTimes = App.DbContext.Inspections
                    .Where(r => r.DatetimePlanned.Date == _selectedDate.Value.Date)
                    .Select(r => r.DatetimePlanned)
                    .ToList();

                // Базовые временные слоты (в формате часов)
                List<TimeSpan> timeSlots = new List<TimeSpan>
                {
                    new TimeSpan(10, 0, 0), // 10:00
                    new TimeSpan(12, 0, 0), // 12:00
                    new TimeSpan(14, 0, 0), // 14:00
                    new TimeSpan(16, 0, 0)  // 16:00
                };

                // Создаем полные DateTime (дата + время)
                List<DateTime> allSlots = timeSlots
                    .Select(time => _selectedDate.Value.Date.Add(time)) // Комбинируем дату и время
                    .ToList();

                // Получаем занятые слоты на эту дату
                List<DateTime> bookedSlots = App.DbContext.Inspections
                    .Where(i => i.InspectorId == _inspector.EmployeeId)
                    .Where(r => r.DatetimePlanned.Date == _selectedDate.Value.Date)
                    .Select(r => r.DatetimePlanned)
                    .ToList();

                // Фильтруем доступные слоты
                List<DateTime> availableSlots = allSlots
                    .Where(slot =>
                        slot > DateTime.Now && // Исключаем прошедшие слоты
                        !bookedSlots.Any(booked => booked.TimeOfDay == slot.TimeOfDay)
                    )
                    .ToList();

                // Отображаем слоты либо выводим предупреждение
                if (availableSlots.Count > 0)
                {
                    timeCB.Visibility = Visibility.Visible; // Делаем комбобокс со слотами видимым
                    timeCB.ItemsSource = availableSlots; // Показываем доступные слоты
                }
                else
                {
                    timeCB.Visibility = Visibility.Collapsed; // Прячем комбобокс со слотами 
                } 
            }
        }

        private void CreateApplication()
        {
            // Получаем инфо для инспекции
            _selectedDate = (DateTime?)dateCB.SelectedItem;
            _selectedTime = (DateTime?)timeCB.SelectedItem;

            // Проверяем данные для формирования инспекции
            bool hasNullFields = _selectedDate == null || _selectedTime == null;

            if (hasNullFields)
            {
                MessageHelper.MessageNullFields();
                return;
            }

            // Подтверждение 
            bool accept = MessageHelper.ConfirmAcceptApplication();
            if (!accept) return;

            // Формирование 
            ApplicationService.Accept(_application, _inspector, (DateTime)_selectedTime);

            // Смена флажка о сохранении
            Saved = true;
            Close();
        }


        // Обработчики событий
        private void Exit_Click(object sender, RoutedEventArgs e) => MessageHelper.ConfirmExit(this);

        private void Add_Click(object sender, RoutedEventArgs e) => CreateApplication();

        private void DateCB_SelectionChanged(object sender, SelectionChangedEventArgs e) => VisibleTimeComboBox();
    }
}
