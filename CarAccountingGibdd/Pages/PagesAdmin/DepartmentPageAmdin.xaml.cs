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

namespace CarAccountingGibdd.Pages.PagesAdmin
{
    /// <summary>
    /// Логика взаимодействия для DepartmentPageAmdin.xaml
    /// </summary>
    public partial class DepartmentPageAmdin : Page
    {
        private DepartmentDataService _dataService;
        public DepartmentPageAmdin()
        {
            InitializeComponent();

            _dataService = new(sorterCB, searchTB, ascendingCHB, searchBTN, resetFiltersBTN, UpdateIC);
            UpdateIC();
        }

        // Методы
        private void UpdateIC()
        {
            var departments = App.DbContext.Departments.Where(r => r.Deleted != 1).ToList();

            // Фильтры
            departments = _dataService.ApplySort(departments);
            departments = _dataService.ApplySearch(departments);

            itemsDG.ItemsSource = null;
            itemsDG.ItemsSource = departments;
        }

        private void Delete()
        {
            bool confirm = MessageHelper.ConfirmDeleteDepartment();
            if (confirm)
            {
                var department = itemsDG.SelectedItem as Department;

                department.Deleted = 1;

                App.DbContext.Update(department);
                App.DbContext.SaveChanges();

                UpdateIC();
            }
        }

        // Обработчики событий
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var department = itemsDG.SelectedItem as Department;

            EditDepartmentsDialog dialog = new(department);
            ComponentsHelper.ShowDialogWindowDark(dialog);

            bool saved = dialog.Saved;
            if (saved) UpdateIC();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddDepartmentsDialog dialog = new();
            ComponentsHelper.ShowDialogWindowDark(dialog);

            bool saved = dialog.Saved;
            if (saved) UpdateIC();
        }

        private void Delete_Click(object sender, RoutedEventArgs e) => Delete();
    }
}
