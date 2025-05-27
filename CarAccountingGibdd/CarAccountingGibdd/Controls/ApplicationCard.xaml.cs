using CarAccountingGibdd.Classes;
using CarAccountingGibdd.Dialogs;
using CarAccountingGibdd.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CarAccountingGibdd.Controls
{
    /// <summary>
    /// Логика взаимодействия для ApplicationCard.xaml
    /// </summary>
    public partial class ApplicationCard : UserControl
    {
        // Свойства
        public event EventHandler<ApplicationEventArgs> ApplicationToAccept;
        public Application Application { get; private set; }

        // Поля
        private Application _application;
        private Employee _employee;

        // Конструктор
        public ApplicationCard(Application application, Employee employee)
        {
            InitializeComponent();

            _application = application;
            _employee = employee;

            DifferentiationFunctionality();
            LoadCardData();
        }

        // Свойства
        private void DifferentiationFunctionality()
        {
            // Разграничиваем функционал по должности и статусам
            int post = _employee.PostId;
            int status = _application.ApplicationStatusId;

            if (post == 2 && status == 2) // инспектор, если еще нет осмотра
            {
                acceptBTN.Visibility = System.Windows.Visibility.Visible;
            }
            else if (post == 3 && status == 1) // оператор, если еще не подтверждена
            {
                confirmationBTNS.Visibility = System.Windows.Visibility.Visible;
            }
            else // все остальные случаи
            {
                infoBTN.Visibility = System.Windows.Visibility.Visible;
                Opacity = 0.5;
            }
        }

        private void LoadCardData()
        {
            App.DbContext.Attach(_application);
            DataContext = _application;
        }

        // Обработчики событий
        private void Card_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InfoApplicationDialog dialog = new(_application);
            ComponentsHelper.ShowDialogWindowDark(dialog);
        }        
        
        private void GetInfo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            e.Handled = true; // Останавливаем всплытие события
            InfoApplicationDialog dialog = new(_application);
            ComponentsHelper.ShowDialogWindowDark(dialog);
        }

        private void Edit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            e.Handled = true; // Останавливаем всплытие события
            // Редактирование
        }

        private void Cancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            e.Handled = true; // Останавливаем всплытие события
            // Добавить логику отмены
            ApplicationToAccept.Invoke(this, new ApplicationEventArgs { Application = this.Application });
        }

        private void ToAccept_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            e.Handled = true; // Останавливаем всплытие события
            // Добавить логику принятия
            //ApplicationToAccept.Invoke(this, new ApplicationEventArgs { Application = this.Application });
        }

        private void AcceptBTN_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
