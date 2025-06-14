using CarAccountingGibdd.Classes.Services;
using CarAccountingGibdd.Classes;
using CarAccountingGibdd.Controls;
using CarAccountingGibdd.Dialogs;
using CarAccountingGibdd.Model;
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

namespace CarAccountingGibdd.Pages.PagesOperator
{
    /// <summary>
    /// Логика взаимодействия для ApplicationPageOperator.xaml
    /// </summary>
    public partial class ApplicationPageOperator : Page
    {
        //  Поля и свойства 
        private Employee _operator;
        private ApplicationDataService _dataService;

        public ApplicationPageOperator(Employee @operator)
        {
            InitializeComponent();

            _operator = @operator;
            _dataService = new(filterCB, sorterCB, searchTB, ascendingCHB, searchBTN, resetFiltersBTN, UpdateIC);
            UpdateIC();
        }

        // Методы
        private void UpdateIC()
        {
            var applications = App.DbContext.Applications.ToList();

            // Фильтры
            applications = _dataService.ApplyFilter(applications, _operator);
            applications = _dataService.ApplySort(applications);
            applications = _dataService.ApplySearch(applications);

            cardsIC.Items.Clear();
            foreach (var application in applications)
            {
                var card = new ApplicationCard(application, _operator);
                card.ApplicationToAcceptEvent += ApplicationToAccept;
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
