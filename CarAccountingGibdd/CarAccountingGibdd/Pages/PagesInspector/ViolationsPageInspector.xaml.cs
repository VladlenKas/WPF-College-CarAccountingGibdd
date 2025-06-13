using CarAccountingGibdd.Classes.Services;
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

namespace CarAccountingGibdd.Pages.PagesInspector
{
    /// <summary>
    /// Логика взаимодействия для ViolationsPageInspector.xaml
    /// </summary>
    public partial class ViolationsPageInspector : Page
    {
        // Поля
        private Employee _inspector;
        private ViolationDataService _dataService;

        // Конструктор
        public ViolationsPageInspector(Employee inspector)
        {
            InitializeComponent();

            _inspector = inspector;
            _dataService = new(sorterCB, searchTB, ascendingCHB, searchBTN, resetFiltersBTN, UpdateIC);
            UpdateIC();
        }

        // Методы
        private void UpdateIC()
        {
            var violations = App.DbContext.Violations.Where(r => r.Deleted != 1).ToList();

            // Фильтры
            violations = _dataService.ApplySort(violations);
            violations = _dataService.ApplySearch(violations);

            itemsDG.ItemsSource = null;
            itemsDG.ItemsSource = violations;
        }
    }
}
