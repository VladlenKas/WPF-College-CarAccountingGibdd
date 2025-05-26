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

        // Конструктор
        public ApplicationCard(Application application)
        {
            InitializeComponent();

            _application = application;
            // Если осмотр не назначен
            if (_application.ApplicationStatusId == 1)
            {
                buttonsDP.Visibility = System.Windows.Visibility.Visible;
            }
            // Если назначен или уже был
            else
            {
                infoBTN.Visibility = System.Windows.Visibility.Visible;
                this.Opacity = 0.5;
            }
            LoadCardData();
        }

        // Свойства
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

        private void Cancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            e.Handled = true; // Останавливаем всплытие события
            // Добавиь логику отмены
            //ApplicationToAccept.Invoke(this, new ApplicationEventArgs { Application = this.Application });
        }

        private void ToAccept_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            e.Handled = true; // Останавливаем всплытие события
            // Добавиь логику принятия
            //ApplicationToAccept.Invoke(this, new ApplicationEventArgs { Application = this.Application });
        }
    }
}
