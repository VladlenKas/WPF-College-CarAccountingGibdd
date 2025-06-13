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
    /// Логика взаимодействия для OwnerPageOperator.xaml
    /// </summary>
    public partial class OwnerPageOperator : Page
    {
        private OwnersDataService _dataService;
        public OwnerPageOperator()
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

        // Обработчики событий
        private void CarInfo_Click(object sender, RoutedEventArgs e)
        {
            var owner = itemsDG.SelectedItem as Owner;

            InfoOwnerDialog dialog = new(owner);
            ComponentsHelper.ShowDialogWindowDark(dialog);
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var owner = itemsDG.SelectedItem as Owner;

            EditOwnerDialog dialog = new(owner);
            ComponentsHelper.ShowDialogWindowDark(dialog);

            bool saved = dialog.Saved;
            if (saved) UpdateIC();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddOwnerDialog dialog = new();
            ComponentsHelper.ShowDialogWindowDark(dialog);

            bool saved = dialog.Saved;
            if (saved) UpdateIC();
        }
    }
}
