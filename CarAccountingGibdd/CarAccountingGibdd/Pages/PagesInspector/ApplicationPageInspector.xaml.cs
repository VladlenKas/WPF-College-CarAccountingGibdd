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
        private Employee _inspector;
        private ApplicationDataService _dataService;

        public ApplicationPageInspector(Employee inspector)
        {
            InitializeComponent();

            _inspector = inspector;
            _dataService = new(filterCB, sorterCB, searchTB, ascendingCHB, searchBTN, resetFiltersBTN, UpdateIC);
            UpdateIC();
        }

        // Методы
        private void UpdateIC()
        {
            var applications = App.DbContext.Applications.ToList();

            // Фильтры
            applications = _dataService.ApplyAccessControl(applications, _inspector);
            applications = _dataService.ApplyFilter(applications);
            applications = _dataService.ApplySort(applications);
            applications = _dataService.ApplySearch(applications);

            cardsIC.Items.Clear();
            foreach (var application in applications)
            {
                var card = new ApplicationCard(application, _inspector);
                card.ApplicationToAcceptEvent += ApplicationToAccept;
                cardsIC.Items.Add(card);
            }
        }

        // Обработчики событий
        private void ApplicationToAccept(object sender, ApplicationEventArgs e) => UpdateIC();
    }
}
