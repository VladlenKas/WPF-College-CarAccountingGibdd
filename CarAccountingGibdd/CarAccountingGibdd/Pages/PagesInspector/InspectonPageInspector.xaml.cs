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

namespace CarAccountingGibdd.Pages.PagesInspector
{
    /// <summary>
    /// Логика взаимодействия для InspectonPageInspector.xaml
    /// </summary>
    public partial class InspectonPageInspector : Page
    {
        //  Поля и свойства 
        private Employee _inspector;

        public InspectonPageInspector(Employee inspector)
        {
            InitializeComponent();

            _inspector = inspector;
            // Фильтры
            UpdateIC();
        }

        // Методы
        private void UpdateIC()
        {
            var inspections = App.DbContext.Inspections.ToList();

            // Фильтры
            /*orders = _orderDataService.ApplyCourier(orders, _thisCourier);
            orders = _orderDataService.ApplyFilter(orders);
            orders = _orderDataService.ApplySort(orders);
            orders = _orderDataService.ApplySearch(orders);*/

            cardsIC.Items.Clear();
            foreach (var inspection in inspections)
            {
                var card = new InspectionCard(inspection, _inspector);
                card.InspectionToAcceptEvent += InspectionToAccept;
                cardsIC.Items.Add(card);
            }
        }

        // Обработчики событий
        private void InspectionToAccept(object sender, InspectionEventArgs e) => UpdateIC();
    }
}
