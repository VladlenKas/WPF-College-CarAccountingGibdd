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
    /// Логика взаимодействия для EmployeePageAdmin.xaml
    /// </summary>
    public partial class EmployeePageAdmin : Page
    {
        // Поля
        private Employee _admin;
        private EmployeesDataService _dataService;

        // Конструктор
        public EmployeePageAdmin(Employee admin)
        {
            InitializeComponent();

            _admin = admin;
            _dataService = new(filterCB, sorterCB, searchTB, ascendingCHB, searchBTN, resetFiltersBTN, UpdateIC);
            UpdateIC();
        }

        // Методы
        private void UpdateIC()
        {
            var employees = App.DbContext.Employees.Where(r => r.Deleted != 1).ToList();

            // Фильтры
            employees = _dataService.ApplyFilter(employees);
            employees = _dataService.ApplySort(employees);
            employees = _dataService.ApplySearch(employees);

            itemsDG.ItemsSource = null;
            itemsDG.ItemsSource = employees;
        }

        private void Delete()
        {
            var employee = itemsDG.SelectedItem as Employee;
            if (_admin.EmployeeId == employee.EmployeeId)
            {
                MessageHelper.MessageUniversal("Вы не можете удалить сами себя!");
                return;
            }

            bool confirm = MessageHelper.ConfirmDeleteEmployee();
            if (confirm)
            {
                employee.Deleted = 1;

                App.DbContext.Update(employee);
                App.DbContext.SaveChanges();

                UpdateIC();
            }
        }

        // Обработчики событий
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var employee = itemsDG.SelectedItem as Employee;

            EditEmployeeDialog dialog = new(employee, _admin);
            ComponentsHelper.ShowDialogWindowDark(dialog);

            bool saved = dialog.Saved;
            if (saved) UpdateIC();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddEmployeeDialog dialog = new();
            ComponentsHelper.ShowDialogWindowDark(dialog);

            bool saved = dialog.Saved;
            if (saved) UpdateIC();
        }

        private void Delete_Click(object sender, RoutedEventArgs e) => Delete();
    }
}
