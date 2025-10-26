using CarAccountingGibdd.Classes.Services;
using CarAccountingGibdd.Classes;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CarAccountingGibdd.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для AddViolationDialog.xaml
    /// </summary>
    public partial class AddViolationDialog : Window
    {
        // Поля и свойства
        public bool Saved { get; private set; } // Флаг сохранения

        // Конструктор
        public AddViolationDialog()
        {
            InitializeComponent();
        }

        // Методы
        private void Add()
        {
            // Получаем данные для добавления владельца
            string description = descriptionTB.Text;

            // Создаем экземпляр сервиса
            ViolationService service = new ViolationService(description);

            // Проверка
            bool notError = service.Check();
            if (!notError) return;

            // Подтверждение
            bool accept = MessageHelper.ConfirmSave();
            if (!accept) return;

            // Формирование
            service.Add();

            // Закрываем и сменяем флажок 
            Saved = true;
            Close();
        }

        // Обработчики событий
        private void Exit_Click(object sender, RoutedEventArgs e) => MessageHelper.ConfirmExit(this);

        private void Add_Click(object sender, RoutedEventArgs e) => Add();
    }
}
