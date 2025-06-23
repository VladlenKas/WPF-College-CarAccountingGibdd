using CarAccountingGibdd.Classes;
using CarAccountingGibdd.Classes.Services;
using CarAccountingGibdd.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Shapes;

namespace CarAccountingGibdd.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для SaveDocumentDialog.xaml
    /// </summary>
    public partial class SaveDocumentDialog : Window
    {
        private string _filepath;
        private string _employeeFullname;

        private Certificate _certificate;
        private IGrouping<int, ViolationInspection> _violationInspections;
        private List<Report> _reports;
        private List<ReportItem> _reportItems;
        private DateOnly _startDate;
        private DateOnly _endDate;

        // Конструктор для свидетельств
        public SaveDocumentDialog(Certificate certificate, Employee employee)
        {
            InitializeComponent();
            _certificate = certificate;
            _employeeFullname = employee.Fullname;
        }

        // Конструктор для списка с нарушениями
        public SaveDocumentDialog(IGrouping<int, ViolationInspection> violationInspections, Employee employee)
        {
            InitializeComponent();
            _violationInspections = violationInspections;
            _employeeFullname = employee.Fullname;
        }

        // Конструктор для отчета Pdf
        public SaveDocumentDialog(List<Report> reports, DateOnly startDate, DateOnly endDate, Employee employee)
        {
            InitializeComponent();
            _startDate = startDate;
            _endDate = endDate;
            _reports = reports;
            _employeeFullname = employee.Fullname;
        }

        // Методы
        private void OpenSaveFileDialog()
        {
            SaveFileDialog saveFileDialog = null;

            if (_certificate != null) // Для сертификата
            {
                // Выбор пути
                saveFileDialog = new SaveFileDialog()
                {
                    Filter = "Pdf Files|*.pdf",
                    Title = "Сохранить PDF документ",
                    FileName = $"Свидетельство о регистрации транспортного средства №{_certificate.CertificateId}"
                };
            }
            else if (_violationInspections != null) // Для списка с нарушениями
            {
                // Выбор пути
                saveFileDialog = new SaveFileDialog()
                {
                    Filter = "Pdf Files|*.pdf",
                    Title = "Сохранить PDF документ",
                    FileName = $"Отчёт о выявленных нарушениях №{_violationInspections.First().InspectionId}"
                };
            }
            else if (_reports != null) // Для отчета Pdf
            {
                // Выбор пути
                saveFileDialog = new SaveFileDialog()
                {
                    Filter = "Pdf Files|*.pdf",
                    Title = "Сохранить PDF документ",
                    FileName = $"Отчет за период с {_startDate:dd.MM.yyyy} по {_endDate:dd.MM.yyyy}"
                };
            }

            // Если пользователь выбрал путь для сохранения чека
            if (saveFileDialog.ShowDialog() == true)
            {
                _filepath = saveFileDialog.FileName;
                filepathTB.Text = $"Путь к документу: {_filepath}";
            }
        }

        private void SaveDocument()
        {
            // Генерируем документ
            if (_certificate != null) // Для сертификата
            {
                DocumentService.GenerateCertificateReport(_filepath, _certificate, _employeeFullname);
            }
            else if (_violationInspections != null) // Для списка с нарушениями
            {
                Inspection inspecton = _violationInspections.First().Inspection;
                DocumentService.GenerateViolationsReport(_filepath, inspecton.InspectionId, _employeeFullname);
            }
            else if (_reports != null)
            {
                DocumentService.GeneratePdfReport(_filepath, _reports, _startDate, _endDate, _employeeFullname);
            }

            // Открываем файл (или нет)
            bool openedDocument = openedDocumentCB.IsChecked.Value;
            if (openedDocument == true)
            {
                if (File.Exists(_filepath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = _filepath,
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show(
                        "Файл не найден. Сохранение было отменено или произошла ошибка.",
                        "Ошибка открытия файла",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }

            Close();
        }

        // Обработчики событий
        private void Exit_Click(object sender, RoutedEventArgs e) => Close();

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на то, что путь выбран
            if (string.IsNullOrEmpty(_filepath))
            {
                MessageHelper.MessageNullFilepath();
                return;
            }

            SaveDocument();
            Close();
        }

        private void FilepathTB_Click(object sender, RoutedEventArgs e)
        {
            OpenSaveFileDialog();
        }
    }
}
