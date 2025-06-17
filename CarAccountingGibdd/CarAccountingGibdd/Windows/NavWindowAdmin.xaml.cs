using CarAccountingGibdd.Pages.PagesAdmin;
using CarAccountingGibdd.Model;
using Microsoft.EntityFrameworkCore;
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
using CarAccountingGibdd.Pages.PagesInspector;

namespace CarAccountingGibdd.Windows
{
    /// <summary>
    /// Логика взаимодействия для NavigationAdminWindow.xaml
    /// </summary>
    public partial class NavWindowAdmin : Window
    {
        // Поля и свойства
        private readonly GibddContext _dbContext;
        private readonly Employee _admin;

        // Конструктор
        public NavWindowAdmin(Employee admin)
        {
            _admin = admin;
            _dbContext = new();

            InitializeComponent();
            Title = $"Меню Администратора. Сотрудник: {_admin.Fullname}";
            FIcourier.Text = admin.FIname;

            ownerRB.IsChecked = true;
            App.MenuWindow = this;
        }

        // Обработчики событий
        private void OwnerRButton_Checked(object sender, RoutedEventArgs e)
        {
            CurrentPage.Navigate(new OwnerPageAdmin());
            titlePage.Text = "Владельцы";
        }
        
        private void VehicleRButton_Checked(object sender, RoutedEventArgs e)
        {
            CurrentPage.Navigate(new VehiclePageAdmin());
            titlePage.Text = "Транспорты";
        }
        
        private void EmployeeRButton_Checked(object sender, RoutedEventArgs e)
        {
            CurrentPage.Navigate(new EmployeePageAdmin(_admin));
            titlePage.Text = "Сотрудники";
        }
        
        private void DepartmentRButton_Checked(object sender, RoutedEventArgs e)
        {
            CurrentPage.Navigate(new DepartmentPageAmdin());
            titlePage.Text = "Департаменты";
        }

        private void ViolationPageRB_Checked(object sender, RoutedEventArgs e)
        {
            CurrentPage.Navigate(new ViolationsPageAdmin());
            titlePage.Text = "Нарушения";
        }

        private void ReportRButton_Checked(object sender, RoutedEventArgs e)
        {
            CurrentPage.Navigate(new ReportPageAdmin(_admin));
            titlePage.Text = "Просмотр отчета";
        }

        private void ExitRButton_Checked(object sender, RoutedEventArgs e)
        {
            AuthWindow window = new();
            window.Show();
            Close();
        }
    }
}
