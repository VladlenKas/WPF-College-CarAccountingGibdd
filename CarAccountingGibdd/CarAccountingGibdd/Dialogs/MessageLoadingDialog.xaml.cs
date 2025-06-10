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
using System.Windows.Threading;

namespace CarAccountingGibdd.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для MessageLoadingDialog.xaml
    /// </summary>
    public partial class MessageLoadingDialog : Window, INotifyPropertyChanged
    {
        // Поля
        private int _secondsLeft;
        private readonly DispatcherTimer _dispatcherTimer;

        // Свойства
        public string SecondsLeft => _secondsLeft.ToString();

        // Конструктор
        public MessageLoadingDialog()
        {
            InitializeComponent();

            DataContext = this;

            _secondsLeft = 6; // Показываем, что осталось 5 секунд

            _dispatcherTimer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1), // Делаем интервал 1 секунду
            };
            _dispatcherTimer.Tick += DispatcherTimer_Tick; // Привызяываем событие, которое будет срабатывать каждый тик
                                                           // В нашем случае это каждую секунду (прописали выше)
            _dispatcherTimer.Start(); // Запускаем таймер
        }

        // Методы
        private void TimerCallback()
        {
            loadGif.Visibility = Visibility.Collapsed; // Убираем гифку
            exitBTN.Visibility = Visibility.Visible; // Показываем кнопку для выхода
        }

        // Обработчики событий
        private void DispatcherTimer_Tick(object? sender, EventArgs e)
        {
            _secondsLeft--; // Уменьшаем секунды для текстбокса
            OnPropertyChanged(nameof(SecondsLeft)); // Обновляем UI

            if (_secondsLeft <= 0) // Если таймер закончился, то
            {
                _dispatcherTimer.Stop(); // Останавливаем его

                timerTB.Text = string.Empty; // Очищаем текст
                infoTB.Text = "Оплата произведена успешно!";

                TimerCallback(); // Вызываем конечный метод
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e) => Close();

        // Реализация интерфейса
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
