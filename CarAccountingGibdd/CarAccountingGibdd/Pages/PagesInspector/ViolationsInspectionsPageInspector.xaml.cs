using CarAccountingGibdd.Controls;
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
    /// Логика взаимодействия для ViolationsInspectionsPageInspector.xaml
    /// </summary>
    public partial class ViolationsInspectionsPageInspector : Page
    {        
        public ViolationsInspectionsPageInspector()
        {
            InitializeComponent();

            // Фильтры
            UpdateIC();
        }

        // Методы
        private void UpdateIC()
        {
            var violationsInspections = App.DbContext.ViolationInspection
                .GroupBy(r => r.InspectionId)
                .ToList();

            // Фильтры
            /*orders = _orderDataService.ApplyCourier(orders, _thisCourier);
            orders = _orderDataService.ApplyFilter(orders);
            orders = _orderDataService.ApplySort(orders);
            orders = _orderDataService.ApplySearch(orders);*/

            cardsIC.Items.Clear();
            foreach (var violationsInspection in violationsInspections)
            {
                cardsIC.Items.Add(new ViolationsInspectionCard(violationsInspection));
            }
        }
    }
}
