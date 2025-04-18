﻿using CarAccoutingGibdd.Classes;
using CarAccoutingGibdd.Model;
using CarAccoutingGibdd.Windows;
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

namespace CarAccoutingGibdd
{
    /// <summary>
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        // Поля и свойства
        private GibddContext _dbContext;
        private string Login => loginTB.Text;
        private string Password => PasswordHelper.GetPassword(PassPB, PassTB);

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
                MessageBox.Show($"Заполните все поля", "Предупреждение.",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    NavigationAdminWindow navigationAdminWindow = new(employee);
                    navigationAdminWindow.Show();
                    Close();
                }
                else
                {
                    // Вход инспектора
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
            DialogHelper.ConfirmExit(this);
        }

        private void VisibilityPassword_Click(object sender, RoutedEventArgs e)
        {
            PasswordHelper.ToggleVisibility(sender, PassPB, PassTB);
        }
    }
}
