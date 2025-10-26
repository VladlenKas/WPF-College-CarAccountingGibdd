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
    /// Логика взаимодействия для EditEmployeeDialog.xaml
    /// </summary>
    public partial class EditEmployeeDialog : Window
    {
        // Свойства
        public bool Saved { get; private set; } // Флаг сохранения

        // Поля
        private Employee _employee;

        // Конструктор
        public EditEmployeeDialog(Employee employee, Employee admin)
        {
            InitializeComponent();

            departmentCB.ItemsSource = App.DbContext.Departments.ToList();
            postCB.ItemsSource = App.DbContext.Posts.ToList();

            firstnameTB.Text = employee.Firstname;
            lastnameTB.Text = employee.Lastname;
            patronymicTB.Text = employee.Patronymic;
            dateTB.DateText = employee.Birthdate.ToString();
            passportTB.Text = employee.Passport;
            postCB.SelectedItem = employee.Post;
            departmentCB.SelectedItem = employee.Department;
            emailTB.Text = employee.Email;
            PassTB.Text = employee.Password;
            PassPB.Password = employee.Password;

            _employee = employee;

            // Блокируем изменение почты у себя
            if (admin.EmployeeId == employee.EmployeeId)
            {
                emailTB.IsEnabled = false;
                emailTB.Opacity = 0.5;
            }
        }

        // Методы
        private async void Edit()
        {
            // Получаем данные для добавления владельца
            string firstname = firstnameTB.Text;
            string lastname = lastnameTB.Text;
            string patronymic = patronymicTB.Text;
            DateOnly birthdate = TypeHelper.DateOnlyParse(dateTB.DateText);
            string passport = passportTB.Text;
            Department? department = departmentCB.SelectedItem as Department;
            Post? post = postCB.SelectedItem as Post;
            string email = emailTB.Text;
            string password = ComponentsHelper.GetPassword(PassPB, PassTB);

            // Создаем экземпляр сервиса
            EmployeeService service = new EmployeeService(firstname, lastname, patronymic, birthdate, passport, department, post, email, password);

            // Проверка
            bool notError = await service.CheckAsync(_employee);
            if (!notError) return;

            // Подтверждение
            bool accept = MessageHelper.ConfirmEdit();
            if (!accept) return;

            // Формирование
            service.Update(_employee);

            // Закрываем и сменяем флажок 
            Saved = true;
            Close();
        }

        // Обработчики событий
        private void Exit_Click(object sender, RoutedEventArgs e) => MessageHelper.ConfirmExit(this);

        private void Add_Click(object sender, RoutedEventArgs e) => Edit();

        private void VisibilityPassword_Click(object sender, RoutedEventArgs e)
        {
            ComponentsHelper.ToggleVisibility(sender, PassPB, PassTB);  // Переключить видимость пароля
        }
    }
}
