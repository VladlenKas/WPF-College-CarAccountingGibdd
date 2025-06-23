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
    /// Логика взаимодействия для AddEmployeeDialog.xaml
    /// </summary>
    public partial class AddEmployeeDialog : Window
    {
        // Поля и свойства
        public bool Saved { get; private set; } // Флаг сохранения

        // Конструктор
        public AddEmployeeDialog()
        {
            InitializeComponent();

            departmentCB.ItemsSource = App.DbContext.Departments.ToList();
            postCB.ItemsSource = App.DbContext.Posts.ToList();
        }

        // Методы
        private async void Add()
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
            bool notError = await service.CheckAsync();
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

        private void VisibilityPassword_Click(object sender, RoutedEventArgs e) => ComponentsHelper.ToggleVisibility(sender, PassPB, PassTB);
    }
}
