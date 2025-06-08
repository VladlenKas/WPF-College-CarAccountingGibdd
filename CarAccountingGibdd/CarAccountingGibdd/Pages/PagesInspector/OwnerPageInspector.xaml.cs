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

namespace CarAccountingGibdd.Pages.PagesInspector
{
    /// <summary>
    /// Логика взаимодействия для OwnerPageInspector.xaml
    /// </summary>
    public partial class OwnerPageInspector : Page
    {
        public OwnerPageInspector()
        {
            InitializeComponent();

            // Фильтры
            UpdateIC();
        }

        // Методы
        private void UpdateIC()
        {
            var owners = App.DbContext.Owners.ToList();

            // Фильтры
            /*orders = _orderDataService.ApplyCourier(orders, _thisCourier);
            orders = _orderDataService.ApplyFilter(orders);
            orders = _orderDataService.ApplySort(orders);
            orders = _orderDataService.ApplySearch(orders);*/

            itemsDG.ItemsSource = null;
            itemsDG.ItemsSource = owners;
        }

        private void CarInfo_Click(object sender, RoutedEventArgs e)
        {
            var owner = itemsDG.SelectedItem as Owner;

            InfoOwnerDialog dialog = new(owner);
            ComponentsHelper.ShowDialogWindowDark(dialog);
        }
    }
}