using CarAccountingGibdd.Classes;
using CarAccountingGibdd.Classes.Services;
using CarAccountingGibdd.Controls;
using CarAccountingGibdd.Dialogs;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CarAccountingGibdd.Pages.PagesInspector
{
    /// <summary>
    /// Логика взаимодействия для ApplicationPageInspector.xaml
    /// </summary>
    public partial class ApplicationPageInspector : Page
    {
        //  Поля и свойства 
        private ApplicationService _service;
        private Employee _inspector;

        public ApplicationPageInspector(Employee inspector)
        {
            InitializeComponent();

            _inspector = inspector;
            // Фильтры
            UpdateIC();
        }

        // Методы
        private void UpdateIC()
        {
            var applications = App.DbContext.Applications.ToList();

            // Фильтры
            /*orders = _orderDataService.ApplyCourier(orders, _thisCourier);
            orders = _orderDataService.ApplyFilter(orders);
            orders = _orderDataService.ApplySort(orders);
            orders = _orderDataService.ApplySearch(orders);*/

            cardsIC.Items.Clear();
            foreach (var application in applications)
            {
                var card = new ApplicationCard(application);
                card.ApplicationToAccept += ApplicationToAccept;

                if (application.ApplicationStatusId != 1)
                {
                    card.Opacity = 0.5;
                }
                cardsIC.Items.Add(card);
            }
        }

        // Обработчики событий
        private void ApplicationToAccept(object sender, ApplicationEventArgs e) => UpdateIC();

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddApplicationDialog dialog = new();
            ComponentsHelper.ShowDialogWindowDark(dialog);

            bool saved = dialog.Saved;
            if (saved) UpdateIC();
        }
    }
}
