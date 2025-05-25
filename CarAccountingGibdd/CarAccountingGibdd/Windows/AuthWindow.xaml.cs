using CarAccountingGibdd.Classes;
using CarAccountingGibdd.Windows;
using CarAccountingGibdd.Model;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CarAccountingGibdd
{
    /// <summary>
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        // Поля и свойства
        private GibddContext _dbContext;
        private string Login => loginTB.Text;
        private string Password => ComponentsHelper.GetPassword(PassPB, PassTB);

        // Конструктор
        public AuthWindow()
        {
            InitializeComponent();
            _dbContext = new();
        }

        // Методы
        private Employee? Authenticate(string login, string password)
        {
            _dbContext.Employees.Include(r => r.Post).Load();

            return _dbContext.Employees.SingleOrDefault(r =>
            r.Login == login && r.Password == password);
        }

        private void Auth()
        {
            if (Login == string.Empty || Password == string.Empty)
            {
                MessageHelper.MessageNullFields();
                return;
            }

            var employee = Authenticate(Login, Password);

            if (employee == null)
            {
                // Если пользователь не найден
                MessageBox.Show("Пользователь с указанными данными не найден. Проверьте логин и пароль.",
                                "Ошибка авторизации",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            else
            {
                // Если пользователь найден
                MessageBox.Show($"Добро пожаловать, {employee.Fullname}!\nВаша должность: {employee.Post.Name}",
                                "Успешная авторизация",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);

                if (employee.PostId == 1)
                {
                    NavWindowAdmin navWindow = new(employee);
                    navWindow.Show();
                    Close();
                }
                else if (employee.PostId == 2)
                {
                    NavWindowInspector navWindow = new(employee);
                    navWindow.Show();
                    Close();
                }
                else if (employee.PostId == 3)
                {
                    NavWindowOperator navWindow = new(employee);
                    navWindow.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Неизвестная ошибка.",
                        "Ошибка авторизации",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        // Тригеры
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Auth();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MessageHelper.ConfirmExit(this);
        }

        private void VisibilityPassword_Click(object sender, RoutedEventArgs e)
        {
            ComponentsHelper.ToggleVisibility(sender, PassPB, PassTB);
        }
    }
}
