using CarAccountingGibdd.Classes;
using CarAccountingGibdd.Classes.Services;
using CarAccountingGibdd.Controls;
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
    /// Логика взаимодействия для InspectionPageOperator.xaml
    /// </summary>
    public partial class InspectionPageOperator : Page
    {
        //  Поля и свойства 
        private Employee _operator;
        private InspectionDataService _dataService;

        public InspectionPageOperator(Employee @operator)
        {
            InitializeComponent();

            _operator = @operator;
            _dataService = new(filterCB, sorterCB, searchTB, ascendingCHB, searchBTN, resetFiltersBTN, UpdateIC);
            UpdateIC();
        }

        // Методы
        private void UpdateIC()
        {
            var inspections = App.DbContext.Inspections.ToList();

            // Фильтры
            inspections = _dataService.ApplyFilter(inspections);
            inspections = _dataService.ApplySort(inspections);
            inspections = _dataService.ApplySearch(inspections);

            cardsIC.Items.Clear();
            foreach (var inspection in inspections)
            {
                var card = new InspectionCard(inspection, _operator);
                cardsIC.Items.Add(card);
            }
        }
    }
}
