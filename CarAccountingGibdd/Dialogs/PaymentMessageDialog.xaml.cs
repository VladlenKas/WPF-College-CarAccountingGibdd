using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CarAccountingGibdd.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для MessageLoadingDialog.xaml
    /// </summary>
    public partial class PaymentMessageDialog : Window, INotifyPropertyChanged
    {
        // Поля
        private bool _isDialogClosed = false;
        private int _secondsLeft;
        private readonly DispatcherTimer _dispatcherTimer;

        // Свойства
        public bool Saved { get; set; } = false;
        public string SecondsLeft => _secondsLeft.ToString();

        // Конструктор
        public PaymentMessageDialog()
        {
            InitializeComponent();

            DataContext = this;

            _secondsLeft = 5; // Показываем, что осталось 5 секунд

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
            stopBTN.Visibility = Visibility.Collapsed; // Убираем кнопку выхода
            loadGif.Visibility = Visibility.Collapsed; // Убираем гифку

            Saved = true; // Помечаем флаг о том, что оплата произведена
            Close();

            MessageBox.Show("Процесс платежа завершен! Формирование заявления прошло успешно!",
                            "Успех",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }

        private void DispatcherTimer_Tick(object? sender, EventArgs e)
        {
            _secondsLeft--; // Уменьшаем секунды для текстбокса
            OnPropertyChanged(nameof(SecondsLeft)); // Обновляем UI

            if (_secondsLeft <= 0) // Если таймер закончился, то
            {
                _dispatcherTimer.Stop(); // Останавливаем его

                timerTB.Text = string.Empty; // Очищаем текст
                //infoTB.Text = "Оплата произведена успешно!";

                if (!_isDialogClosed)
                    TimerCallback(); // Вызываем конечный метод
            }
        }

        // Обработчики событий
        private void Window_Closed(object sender, EventArgs e)
        {
            _isDialogClosed = true;
        }

        private void Exit_Click(object sender, RoutedEventArgs e) => this.Close();

        // Реализация интерфейса
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // Скрыываем крестик на панели окна
        const int MF_BYPOSITION = 0x400;
        const int MF_REMOVE = 0x1000;

        [DllImport("user32.dll")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        static extern bool RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        [DllImport("user32.dll")]
        static extern int GetMenuItemCount(IntPtr hMenu);

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var hwnd = new WindowInteropHelper(this).Handle;
            IntPtr hMenu = GetSystemMenu(hwnd, false);
            int menuItemCount = GetMenuItemCount(hMenu);
            // Удаляет последнюю кнопку (обычно "Закрыть")
            RemoveMenu(hMenu, (uint)(menuItemCount - 1), MF_BYPOSITION | MF_REMOVE);
        }
    }
}
