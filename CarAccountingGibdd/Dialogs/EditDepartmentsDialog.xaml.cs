using CarAccountingGibdd.Classes.Services;
using CarAccountingGibdd.Classes;
using CarAccountingGibdd.Model;
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
    /// Логика взаимодействия для EditDepartmentsDialog.xaml
    /// </summary>
    public partial class EditDepartmentsDialog : Window
    {
        // Свойства
        public bool Saved { get; private set; } // Флаг сохранения

        // Поля
        private Department _department;

        // Конструктор
        public EditDepartmentsDialog(Department department)
        {
            InitializeComponent();

            nameTB.Text = department.Name;
            phoneTB.PhoneNumber = department.Phone;
            addressTB.Text = department.Address;

            _department = department;
        }

        // Методы
        private void Edit()
        {
            // Получаем данные для добавления владельца
            string name = nameTB.Text;
            string phone = phoneTB.PhoneNumber;
            string address = addressTB.Text;

            // Создаем экземпляр сервиса
            DepartmentService service = new DepartmentService(name, phone, address);

            // Проверка
            bool notError = service.Check(_department);
            if (!notError) return;

            // Подтверждение
            bool accept = MessageHelper.ConfirmEdit();
            if (!accept) return;

            // Формирование
            service.Update(_department);

            // Закрываем и сменяем флажок 
            Saved = true;
            Close();
        }

        // Обработчики событий
        private void Exit_Click(object sender, RoutedEventArgs e) => MessageHelper.ConfirmExit(this);

        private void Edit_Click(object sender, RoutedEventArgs e) => Edit();
    }
}
