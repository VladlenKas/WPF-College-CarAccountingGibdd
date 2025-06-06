using CarAccountingGibdd.Pages.PagesAdmin;
using CarAccountingGibdd.Model;
using CarAccountingGibdd;
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
    /// Логика взаимодействия для NavWindowInspector.xaml
    /// </summary>
    public partial class NavWindowInspector : Window
    {
        // Поля и свойства
        private readonly GibddContext _dbContext;
        private readonly Employee _inspector;

        // Конструктор
        public NavWindowInspector(Employee inspector)
        {
            _inspector = inspector;
            _dbContext = new();

            InitializeComponent();
            Title = $"Меню Администратора. Сотрудник: {_inspector.Fullname}";
            FIcourier.Text = inspector.FIname;

            applicationRB.IsChecked = true;
            App.MenuWindow = this;
        }

        // Обработчики событий
        private void ApplicationRButton_Checked(object sender, RoutedEventArgs e)
        {
            CurrentPage.Navigate(new ApplicationPageInspector(_inspector));
            titlePage.Text = "Заявки";
        }
        
        private void InspectionRButton_Checked(object sender, RoutedEventArgs e)
        {
            CurrentPage.Navigate(new InspectionPageInspector(_inspector));
            titlePage.Text = "Осмотры";
        }
        
        private void OwnerRButton_Checked(object sender, RoutedEventArgs e)
        {
            CurrentPage.Navigate(new OwnerPageAdmin());
            titlePage.Text = "Владельцы";
        }

        private void VehicleRButton_Checked(object sender, RoutedEventArgs e)
        {
            CurrentPage.Navigate(new VehiclePageAdmin());
            titlePage.Text = "Автомобили";
        }

        private void EmployeeRButton_Checked(object sender, RoutedEventArgs e)
        {
            CurrentPage.Navigate(new EmployeePageAdmin());
            titlePage.Text = "Сотрудники";
        }
        
        private void ValitionRButton_Checked(object sender, RoutedEventArgs e)
        {
            CurrentPage.Navigate(new ViolationsInspectionsPageInspector());
            titlePage.Text = "Списки нарушений";
        }

        private void DepartmentRButton_Checked(object sender, RoutedEventArgs e)
        {
            CurrentPage.Navigate(new DepartmentPageAmdin());
            titlePage.Text = "Департаменты";
        }
        
        private void CertificateRButton_Checked(object sender, RoutedEventArgs e)
        {
            CurrentPage.Navigate(new DepartmentPageAmdin());
            titlePage.Text = "Свидетельства";
        }

        private void ExitRButton_Checked(object sender, RoutedEventArgs e)
        {
            AuthWindow window = new();
            window.Show();
            Close();
        }

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            // Открытие диалогового окна для создания отчета
        }
    }
}
