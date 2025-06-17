using CarAccountingGibdd.Classes;
using CarAccountingGibdd.Classes.Services;
using CarAccountingGibdd.Dialogs;
using CarAccountingGibdd.Model;
using Microsoft.VisualBasic;
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
    /// Логика взаимодействия для ReportPageAdmin.xaml
    /// </summary>
    public partial class ReportPageAdmin : Page
    {
        // Поля
        private Employee _admin;
        private ReportDataService _dataService;

        // Конструктор
        public ReportPageAdmin(Employee admin)
        {
            InitializeComponent();

            DateOnly endDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly startDate = endDate.AddMonths(-1);

            endDateTB.DateText = endDate.ToString();
            startDateTB.DateText = startDate.ToString();

            _admin = admin;
            _dataService = new(sorterCB, searchTB, ascendingCHB, searchBTN, resetFiltersBTN, startDateTB, endDateTB, UpdateIC);
            UpdateIC();
        }

        // Методы
        private void UpdateIC()
        {
            var reports = App.DbContext.Applications
                .Select(a => new Report
                {
                    ApplcationId = a.ApplicationId,
                    StatusName = a.ApplicationStatus.Name,          // Название статуса заявки
                    OwnerFullname = a.Owner.Fullname,               // ФИО владельца
                    VehicleFullInfo = a.Vehicle.FullInfo,           // Инфо о ТС
                    DepartmentName = a.Department.Name,             // Название отдела
                    DatetimeSupply = a.DatetimeSupply,              // Дата подачи заявки
                    DatetimeConfirm = a.DatetimeConfirm             // Дата подтверждения 
                })
                .ToList();

            // Фильтры
            reports = _dataService.ApplyDateFilter(reports);
            reports = _dataService.ApplySort(reports);
            reports = _dataService.ApplySearch(reports);

            itemsDG.ItemsSource = null;
            itemsDG.ItemsSource = reports;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
/*            AddOwnerDialog dialog = new();
            ComponentsHelper.ShowDialogWindowDark(dialog);

            bool saved = dialog.Saved;
            if (saved) UpdateIC();*/
        }
    }
}