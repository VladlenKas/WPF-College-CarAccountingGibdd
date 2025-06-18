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
        private List<ReportItem> _reportItems;
        private DateOnly _startDate;
        private DateOnly _endDate;

        // Конструктор
        public ReportPageAdmin(Employee admin)
        {
            InitializeComponent();

            _admin = admin;
            _dataService = new(resetFiltersBTN, startDateTB, endDateTB, itemsDG, UpdateIC);

            UpdateIC();
        }

        // Методы
        private void UpdateIC()
        {
            bool isStartValid = DateOnly.TryParse(startDateTB.DateText, out DateOnly startDate);
            bool isEndValid = DateOnly.TryParse(endDateTB.DateText, out DateOnly endDate);

            var combinedReport = CombinedReport.GetCombinedReport(startDate, endDate, App.DbContext);

            var reportItems = new List<ReportItem>
            {
                new ReportItem { Indicator = "Всего заявок", Value = combinedReport.TotalApplications.ToString() },
                new ReportItem { Indicator = "Заявок с активными сертификатами", Value = combinedReport.ActiveCertificateApplications.ToString() },
                new ReportItem { Indicator = "Проведено осмотров", Value = combinedReport.InspectionsCount.ToString() },
                new ReportItem { Indicator = "Зарегистрировано ТС", Value = combinedReport.RegisteredVehicles.ToString() },
                new ReportItem { Indicator = "Выдано свидетельств", Value = combinedReport.IssuedCertificates.ToString() },
                new ReportItem { Indicator = "Выявлено нарушений", Value = combinedReport.ViolationsCount.ToString() },
                new ReportItem { Indicator = "Среднее время обработки заявки (дней)", Value = combinedReport.AverageProcessingTimeDays.ToString("F1") },
                new ReportItem { Indicator = "Процент выданных свидетельств от зарегистрированных ТС", Value = $"{combinedReport.PercentCertificatesFromVehicles:F1}%" },
                new ReportItem { Indicator = "Процент заявок с активными сертификатами", Value = $"{combinedReport.PercentActiveCertApplications:F1}%" },
                new ReportItem { Indicator = "Процент завершённых заявок", Value = $"{combinedReport.PercentCompletedApplications:F1}%" },
                new ReportItem { Indicator = "Процент выявленных нарушений от осмотров", Value = $"{combinedReport.PercentViolationsFromInspections:F1}%" },
                new ReportItem { Indicator = "Процент заявок с нарушениями", Value = $"{combinedReport.PercentApplicationsWithViolations:F1}%" },
            };

            itemsDG.ItemsSource = null;
            itemsDG.ItemsSource = reportItems;

            _startDate = startDate;
            _endDate = endDate;
            _reportItems = reportItems;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            SaveDocumentDialog dialog = new(_reportItems, _startDate, _endDate, _admin);
            ComponentsHelper.ShowDialogWindowDark(dialog);
        }
    }
}