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
using CarAccountingGibdd.Model;

namespace CarAccountingGibdd.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для EditViolationDialog.xaml
    /// </summary>
    public partial class EditViolationDialog : Window
    {
        // Свойства
        public bool Saved { get; private set; } // Флаг сохранения

        // Поля
        private Violation _violation;

        // Конструктор
        public EditViolationDialog(Violation violation)
        {
            InitializeComponent();

            descriptionTB.Text = violation.Description;
            _violation = violation;
        }

        // Методы
        private void Add()
        {
            // Получаем данные для добавления 
            string description = descriptionTB.Text;

            // Создаем экземпляр сервиса
            ViolationService service = new ViolationService(description);

            // Проверка
            bool notError = service.Check(_violation);
            if (!notError) return;

            // Подтверждение
            bool accept = MessageHelper.ConfirmEdit();
            if (!accept) return;

            // Формирование
            service.Update(_violation);

            // Закрываем и сменяем флажок 
            Saved = true;
            Close();
        }

        // Обработчики событий
        private void Exit_Click(object sender, RoutedEventArgs e) => MessageHelper.ConfirmExit(this);

        private void Add_Click(object sender, RoutedEventArgs e) => Add();
    }
}
