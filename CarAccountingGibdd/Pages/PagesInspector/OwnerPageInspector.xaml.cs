using CarAccountingGibdd.Classes;
using CarAccountingGibdd.Classes.Services;
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
        private OwnersDataService _dataService;
        public OwnerPageInspector()
        {
            InitializeComponent();

            _dataService = new(filterCB, sorterCB, searchTB, ascendingCHB, searchBTN, resetFiltersBTN, UpdateIC);
            UpdateIC();
        }

        // Методы
        private void UpdateIC()
        {
            var owners = App.DbContext.Owners.Where(r => r.Deleted != 1).ToList();

            // Фильтры
            owners = _dataService.ApplyFilter(owners);
            owners = _dataService.ApplySort(owners);
            owners = _dataService.ApplySearch(owners);

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