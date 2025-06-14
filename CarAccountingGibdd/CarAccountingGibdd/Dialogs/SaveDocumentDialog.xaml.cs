using CarAccountingGibdd.Classes;
using CarAccountingGibdd.Classes.Services;
using CarAccountingGibdd.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private Certificate _certificate;
        private IGrouping<int, ViolationInspection> _violationInspections;
        private string employeeFullname;

        // Конструктор для свидетельств
        public SaveDocumentDialog(Certificate certificate, Employee employee)
        {
            InitializeComponent();
            _certificate = certificate;
            employeeFullname = employee.Fullname;
        }

        // Конструктор для списка с нарушениями
        public SaveDocumentDialog(IGrouping<int, ViolationInspection> violationInspections, Employee employee)
        {
            InitializeComponent();
            _violationInspections = violationInspections;
            employeeFullname = employee.Fullname;
        }

        // Методы
        private void OpenSaveFileDialog()
        {
            SaveFileDialog saveFileDialog = null;

            // Для сертификата
            if (_certificate != null)
            {
                // Выбор пути
                saveFileDialog = new SaveFileDialog()
                {
                    Filter = "Pdf Files|*.pdf*",
                    Title = "Сохранить PDF документ",
                    FileName = $"Свидетельство о регистрации транспортного средства №{_certificate.CertificateId}"
                };
            }
            
            // Для списка с нарушениями
            if (_violationInspections != null)
            {
                // Выбор пути
                saveFileDialog = new SaveFileDialog()
                {
                    Filter = "Pdf Files|*.pdf*",
                    Title = "Сохранить PDF документ",
                    FileName = $"Нарушения ТС по инспекции №{_violationInspections.First().InspectionId}"
                };
            }

            // Если пользователь выбрал путь для сохранения чека
            if (saveFileDialog.ShowDialog() == true)
            {
                _filepath = $"{saveFileDialog.FileName}.pdf"; // Путь для открытия файла
                filepathTB.Text = $"Путь к документу: {_filepath}";
            }
        }

        private void SaveDocument()
        {
            // Генерируем документ
            if (_certificate != null)
            {
                DocumentService.GenerateCertificate(_filepath, _certificate, employeeFullname);
            }

            // Открываем файл (или нет)
            bool openedDocument = openedDocumentCB.IsChecked.Value;
            if (openedDocument == true)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = _filepath,
                    UseShellExecute = true // Используем оболочку Windows для открытия файла
                });
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
