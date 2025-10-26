using CarAccountingGibdd.Classes.Services;
using CarAccountingGibdd.Classes;
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
using static System.Net.Mime.MediaTypeNames;

namespace CarAccountingGibdd.Pages.PagesInspector
{
    /// <summary>
    /// Логика взаимодействия для InspectonPageInspector.xaml
    /// </summary>
    public partial class InspectionPageInspector : Page
    {
        //  Поля и свойства 
        private Employee _inspector;
        private InspectionDataService _dataService;

        public InspectionPageInspector(Employee inspector)
        {
            InitializeComponent();

            _inspector = inspector;
            _dataService = new(filterCB, sorterCB, searchTB, ascendingCHB, searchBTN, resetFiltersBTN, UpdateIC);
            UpdateIC();
        }

        // Методы
        private void UpdateIC()
        {
            var inspections = App.DbContext.Inspections.ToList();

            // Фильтры
            inspections = _dataService.ApplyAccessControl(inspections, _inspector);
            inspections = _dataService.ApplyFilter(inspections);
            inspections = _dataService.ApplySort(inspections);
            inspections = _dataService.ApplySearch(inspections);

            cardsIC.Items.Clear();
            foreach (var inspection in inspections)
            {
                var card = new InspectionCard(inspection, _inspector);
                card.InspectionToAcceptEvent += ToAccept;
                cardsIC.Items.Add(card);
            }
        }

        // Обработчики событий
        private void ToAccept(object sender, InspectionEventArgs e) => UpdateIC();
    }
}
