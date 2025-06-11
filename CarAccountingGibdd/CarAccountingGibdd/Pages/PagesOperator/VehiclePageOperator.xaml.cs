using CarAccountingGibdd.Classes;
using CarAccountingGibdd.Classes.Services;
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
    /// Логика взаимодействия для VehiclePageOperator.xaml
    /// </summary>
    public partial class VehiclePageOperator : Page
    {
        public VehiclePageOperator()
        {
            InitializeComponent();

            // Фильтры
            UpdateIC();
        }

        // Методы
        private void UpdateIC()
        {
            var owners = App.DbContext.Vehicles.ToList();

            // Фильтры
            /*orders = _orderDataService.ApplyCourier(orders, _thisCourier);
            orders = _orderDataService.ApplyFilter(orders);
            orders = _orderDataService.ApplySort(orders);
            orders = _orderDataService.ApplySearch(orders);*/

            itemsDG.ItemsSource = null;
            itemsDG.ItemsSource = owners;
        }

        // Обработчики событий
        private void DetachOwner_Click(object sender, RoutedEventArgs e)
        {
            var vehicle = itemsDG.SelectedItem as Vehicle;

            if (vehicle.Owner == null)
            {
                MessageHelper.MessageUniversal("У данного ТС нет владельца!");
                return;
            }
            
            if (vehicle.Owner != null)
            {
                bool accept = MessageHelper.ConfirmDetachOwner();
                if (!accept) return;

                VehicleService.DetachOwner(vehicle);
                UpdateIC();
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var vehicle = itemsDG.SelectedItem as Vehicle;

            EditVehicleDialog dialog = new(vehicle);
            ComponentsHelper.ShowDialogWindowDark(dialog);

            bool saved = dialog.Saved;
            if (saved) UpdateIC();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddVehicleDialog dialog = new();
            ComponentsHelper.ShowDialogWindowDark(dialog);

            bool saved = dialog.Saved;
            if (saved) UpdateIC();
        }
    }
}
