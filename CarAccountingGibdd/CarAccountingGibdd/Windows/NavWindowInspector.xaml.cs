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
            Title = $"Меню Инспектора. Сотрудник: {_inspector.Fullname}";
            FIcourier.Text = inspector.FIname;

            applicationRB.IsChecked = true;
            App.MenuWindow = this;
        }

        // Обработчики событий
        private void ApplicationPageRB_Checked(object sender, RoutedEventArgs e)
        {
            CurrentPage.Navigate(new ApplicationPageInspector(_inspector));
            titlePage.Text = "Заявления";
        }
        
        private void InspectionPageRB_Checked(object sender, RoutedEventArgs e)
        {
            CurrentPage.Navigate(new InspectionPageInspector(_inspector));
            titlePage.Text = "Осмотры";
        }

        private void ViolationPageRB_Checked(object sender, RoutedEventArgs e)
        {
            CurrentPage.Navigate(new ViolationsPageInspector(_inspector));
            titlePage.Text = "Нарушения";
        }

        private void OwnerPageRB_Checked(object sender, RoutedEventArgs e)
        {
            CurrentPage.Navigate(new OwnerPageInspector());
            titlePage.Text = "Владельцы";
        }

        private void VehiclePageRB_Checked(object sender, RoutedEventArgs e)
        {
            CurrentPage.Navigate(new VehiclePageInspector());
            titlePage.Text = "Транспорты";
        }
        
        private void ViolationInspectionPageRB_Checked(object sender, RoutedEventArgs e)
        {
            CurrentPage.Navigate(new ViolationsInspectionsPageInspector(_inspector));
            titlePage.Text = "Отчеты нарушений";
        }

        private void CertificatePageRB_Checked(object sender, RoutedEventArgs e)
        {
            CurrentPage.Navigate(new CertificatePageInspector(_inspector));
            titlePage.Text = "Свидетельства";
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow window = new();
            window.Show();
            Close();
        }
    }
}
