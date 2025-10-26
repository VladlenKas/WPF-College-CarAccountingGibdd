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
using CarAccountingGibdd.Classes.Services;
using CarAccountingGibdd.Dialogs;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;

namespace CarAccountingGibdd
{
    /// <summary>
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        // Поля и свойства
        private string Email => emailTB.Text;
        private string Password => ComponentsHelper.GetPassword(PassPB, PassTB);

        // Конструктор
        public AuthWindow()
        {
            InitializeComponent();

            // Проверка подключения к БД при запуске
            try
            {
                using (var context = new GibddContext())
                {
                    var testQuery = context.Employees.Any();
                }
            }
            catch (Exception ex)
            {
                HandleDatabaseError(ex);
            }

            //ApplicationService.HasStartedInspections(); // Проверка на просроченные записи
        }

        // Методы
        private void Auth()
        {
            if (Email == string.Empty || Password == string.Empty)
            {
                MessageHelper.MessageNullFields();
                return;
            }

            var employee = App.DbContext.Employees.SingleOrDefault(r =>
                r.Email == Email && r.Password == Password);

            if (employee == null)
            {
                // Если пользователь не найден
                MessageBox.Show("Пользователь с указанными данными не найден. Проверьте электронную почту и пароль",
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

                switch (employee.PostId)
                {
                    // Админ
                    case 1:
                        NavWindowAdmin adminNavWindow = new(employee);
                        adminNavWindow.Show();
                        Close();
                        break;

                    // Инспектор
                    case 2:
                        NavWindowInspector inspectorNavWindow = new(employee);
                        inspectorNavWindow.Show();
                        Close();
                        break;

                    // Оператор
                    case 3:
                        NavWindowOperator operatorNavWindow = new(employee);
                        operatorNavWindow.Show();
                        Close();
                        break;
                }
            }
        }

        // Обработчики событий
        private void Login_Click(object sender, RoutedEventArgs e) => Auth();

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            var resultChanged = MessageBox.Show("Вы действительно хотите выйти?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resultChanged == MessageBoxResult.Yes)
            {
                System.Windows.Application.Current.Shutdown();
            }
        }

        private void VisibilityPassword_Click(object sender, RoutedEventArgs e) => ComponentsHelper.ToggleVisibility(sender, PassPB, PassTB);

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();

            ChangePasswordDialog dialog = new();
            dialog.ShowDialog();

            this.Show();
        }

        private void HandleDatabaseError(Exception ex)
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");

            string errorMessage = ex.Message;

            if (ex.Message.Contains("Access denied"))
            {
                errorMessage = "Неверный пароль или пользователь MySQL";
            }
            else if (ex.Message.Contains("Unknown database"))
            {
                errorMessage = "База данных «gibdd» не найдена. Импортируйте gibdd.sql";
            }
            else if (ex.Message.Contains("Unable to connect"))
            {
                errorMessage = "MySQL не запущен или недоступен";
            }

            MessageBox.Show(
                "Не удалось подключиться к базе данных\n\n" +
                $"Причина: {errorMessage}\n\n" +
                "Проверьте:\n" +
                "• MySQL установлен и запущен\n" +
                "• База данных «gibdd» создана (gibdd.sql)\n" +
                "• Настройки подключения в конфиге верны\n\n" +
                $"Файл конфигурации:\n{configPath}",
                "Ошибка подключения",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            // Предлагаем открыть конфиг
            if (MessageBox.Show(
                "Открыть файл конфигурации для редактирования?",
                "Помощь",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    System.Diagnostics.Process.Start("notepad.exe", configPath);
                }
                catch   
                {
                    System.Diagnostics.Process.Start("explorer.exe",
                        $"/select,\"{configPath}\"");
                }
            }

            Close();
        }
    }
}
