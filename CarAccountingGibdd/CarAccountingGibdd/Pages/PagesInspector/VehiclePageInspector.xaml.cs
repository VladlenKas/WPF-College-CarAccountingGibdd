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

namespace CarAccountingGibdd.Pages.PagesInspector
{
    /// <summary>
    /// Логика взаимодействия для VehiclePageInspector.xaml
    /// </summary>
    public partial class VehiclePageInspector : Page
    {
        private VehiclesDataService _dataService;
        public VehiclePageInspector()
        {
            InitializeComponent();

            _dataService = new(filterCB, sorterCB, searchTB, ascendingCHB, searchBTN, resetFiltersBTN, UpdateIC);
            UpdateIC();
        }

        // Методы
        private void UpdateIC()
        {
            var vehicles = App.DbContext.Vehicles.Where(r => r.Deleted != 1).ToList();

            // Фильтры
            vehicles = _dataService.ApplyFilter(vehicles);
            vehicles = _dataService.ApplySort(vehicles);
            vehicles = _dataService.ApplySearch(vehicles);

            itemsDG.ItemsSource = null;
            itemsDG.ItemsSource = vehicles;
        }

        // Обработчики событий
        private void ImageView_Click(object sender, RoutedEventArgs e)
        {
            var vehicle = itemsDG.SelectedItem as Vehicle;
            List<PhotosVehicle> photos = vehicle.PhotosVehicles.ToList();

            ViewImageForVehicleDialog dialog = new(photos);
            ComponentsHelper.ShowDialogWindowDark(dialog);
        }
    }
}
