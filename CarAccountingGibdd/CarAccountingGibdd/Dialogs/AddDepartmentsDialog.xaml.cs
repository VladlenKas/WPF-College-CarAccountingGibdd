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

namespace CarAccountingGibdd.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для AddDepartmentsDialog.xaml
    /// </summary>
    public partial class AddDepartmentsDialog : Window
    {
        // Свойства
        public bool Saved { get; private set; } // Флаг сохранения

        // Конструктор
        public AddDepartmentsDialog()
        {
            InitializeComponent();
        }

        // Методы
        private void Add()
        {
            // Получаем данные для добавления владельца
            string name = nameTB.Text;
            string phone = phoneTB.PhoneNumber;
            string address = addressTB.Text;

            // Создаем экземпляр сервиса
            DepartmentService service = new DepartmentService(name, phone, address);

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
