using CarAccountingGibdd.Model;
using CarAccountingGibdd.Pages.PagesInspector;
using CarAccountingGibdd.Pages.PagesOperator;
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

namespace CarAccountingGibdd.Windows
{
    /// <summary>
    /// Логика взаимодействия для NavWindowOpertor.xaml
    /// </summary>
    public partial class NavWindowOperator : Window
    {
        // Поля и свойства
        private readonly GibddContext _dbContext;
        private readonly Employee _operator;

        // Конструктор
        public NavWindowOperator(Employee @operator)
        {
            _operator = @operator;
            _dbContext = new();

            InitializeComponent();
            Title = $"Меню Оператора. Сотрудник: {_operator.Fullname}";
            FIcourier.Text = @operator.FIname;

            applicationRB.IsChecked = true;
            App.MenuWindow = this;
        }

        // Обработчики событий
        private void ApplicationPageRB_Checked(object sender, RoutedEventArgs e)
        {
            CurrentPage.Navigate(new ApplicationPageOperator(_operator));
            titlePage.Text = "Заявки";
        }

        private void InspectionPageRB_Checked(object sender, RoutedEventArgs e)
        {
            CurrentPage.Navigate(new InspectionPageOperator(_operator));
            titlePage.Text = "Осмотры";
        }

        private void OwnerPageRB_Checked(object sender, RoutedEventArgs e)
        {
            CurrentPage.Navigate(new OwnerPageOperator());
            titlePage.Text = "Владельцы";
        }

        private void VehiclePageRB_Checked(object sender, RoutedEventArgs e)
        {
            CurrentPage.Navigate(new VehiclePageOperator());
            titlePage.Text = "Транспорты";
        }

        private void ViolationInspectionPageRB_Checked(object sender, RoutedEventArgs e)
        {
            CurrentPage.Navigate(new ViolationsInspectionsPageOperator(_operator));
            titlePage.Text = "Отчеты нарушений";
        }

        private void CertificatePageRB_Checked(object sender, RoutedEventArgs e)
        {
            CurrentPage.Navigate(new CertificatePageOperator(_operator));
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
