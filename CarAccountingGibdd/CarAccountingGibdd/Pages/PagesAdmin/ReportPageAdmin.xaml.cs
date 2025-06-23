using CarAccountingGibdd.Classes;
using CarAccountingGibdd.Classes.Services;
using CarAccountingGibdd.Dialogs;
using CarAccountingGibdd.Model;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private List<Report> _reports;
        private static readonly Regex DateRegex = new Regex(@"^\d{2}\.\d{2}(\.\d{4})?$");

        // Конструктор
        public ReportPageAdmin(Employee admin)
        {
            InitializeComponent();

            DateOnly endDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly startDate = endDate.AddMonths(-1);

            endDateTB.DateText = endDate.ToString();
            startDateTB.DateText = startDate.ToString();

            _admin = admin;
            _dataService = new(sorterCB, searchTB, ascendingCHB, searchBTN, resetFiltersBTN, startDateTB, endDateTB, itemsDG, UpdateIC);
            UpdateIC();
        }

        // Методы
        private void UpdateIC()
        {
            var reports = App.DbContext.Applications
                .Select(a => new Report
                {
                    ApplcationId = a.ApplicationId,
                    DepartmentName = a.DepartmentName,              // Название отдела
                    OwnerFullname = a.Owner.Fullname,               // ФИО владельца
                    VehicleFullInfo = a.Vehicle.FullInfo,           // Инфо о ТС
                    DatetimeSupply = a.DatetimeSupply,              // Дата подачи заявки
                    DatetimeConfirm = a.DatetimeConfirm,            // Дата подтверждения 
                    StatusName = a.ApplicationStatus.Name           // Название статуса заявки
                })
                .ToList();

            // Фильтры
            reports = _dataService.ApplyDateFilter(reports);
            reports = _dataService.ApplySort(reports);
            reports = _dataService.ApplySearch(reports);

            itemsDG.ItemsSource = null;
            itemsDG.ItemsSource = reports;

            _reports = reports;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            bool isStartValid = DateOnly.TryParse(startDateTB.DateText, out DateOnly startDate);
            bool isEndValid = DateOnly.TryParse(endDateTB.DateText, out DateOnly endDate);

            if (isStartValid && isEndValid)
            {
                if (IsValidDateFormat(startDate.ToString()) && IsValidDateFormat(endDate.ToString()))
                {
                    SaveDocumentDialog dialog = new(_reports, startDate, endDate, _admin);
                    ComponentsHelper.ShowDialogWindowDark(dialog);
                }
            }
            else
            {
                MessageHelper.MessageUniversal("Выберите корректный формат даты!");
            }
        }

        // Фильтрация по датам
        private bool IsValidDateFormat(string dateText)
        {
            return DateRegex.IsMatch(dateText);
        }
    }
}