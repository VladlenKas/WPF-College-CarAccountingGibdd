using CarAccountingGibdd.Classes;
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
using static System.Net.Mime.MediaTypeNames;

namespace CarAccountingGibdd.Pages.PagesAdmin
{
    /// <summary>
    /// Логика взаимодействия для OwnerPageAdmin.xaml
    /// </summary>
    public partial class OwnerPageAdmin : Page
    {
        public OwnerPageAdmin()
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

        private void Delete()
        {
            bool confirm = MessageHelper.ConfirmDeleteOwner();
            if (confirm)
            {
                var owner = itemsDG.SelectedItem as Owner;

                // Делаем все свидетельства недействительными
                owner.Applications?
                    .ToList()
                    .ForEach(a =>
                    {
                        a.Certificates?
                            .Where(c => c.IsActive == 0)
                            .ToList()
                            .ForEach(c => c.IsActive = 1);
                    });

                owner.Deleted = 1;

                App.DbContext.Update(owner);
                App.DbContext.SaveChanges();

                UpdateIC();
            }
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

        private void Delete_Click(object sender, RoutedEventArgs e) => Delete();
    }
}
