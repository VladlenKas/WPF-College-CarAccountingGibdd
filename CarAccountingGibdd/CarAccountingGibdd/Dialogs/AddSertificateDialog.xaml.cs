using CarAccountingGibdd.Classes;
using CarAccountingGibdd.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CarAccountingGibdd.Classes.Services;

namespace CarAccountingGibdd.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для AddSertificateDialog.xaml
    /// </summary>
    public partial class AddSertificateDialog : Window
    {
        // Свойства для контекста
        public string NewLicensePlate { get; set; }
        public int CertificateId { get; set; }
        public string CertificateNumber { get; set; }
        public DateTime DatetimeIssue { get; set; }
        public Inspection Inspection { get; set; }

        // Свойства
        public bool Saved { get; private set; } // Флаг сохранения

        // Конструктор
        public AddSertificateDialog(Inspection inspection)
        {
            InitializeComponent();

            Inspection = inspection;
            NewLicensePlate = GetNewLicensePlate();
            CertificateId = GetCertificateId();
            CertificateNumber = GetRandimCertificateNumber();
            DatetimeIssue = DateTime.Now;

            DataContext = this;
        }

        // Методы
        private int GetCertificateId() =>
            !App.DbContext.Certificates.Any() ? 
            1 : ++(App.DbContext.Certificates.Last().CertificateId);

        private string GetNewLicensePlate()
        {
            var newLicensePlate = new StringBuilder();
            var random = new Random();
            const string letters = "АБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЭЮЯ";

            do
            {
                newLicensePlate.Clear();

                newLicensePlate.Append(
                    $"{letters[random.Next(letters.Length)]}" +
                    random.Next(10) +
                    random.Next(10) +
                    random.Next(10) +
                    $"{letters[random.Next(letters.Length)]}" +
                    $"{letters[random.Next(letters.Length)]}"
                    );
            }
            while (App.DbContext.Certificates.Any(c => c.Number == newLicensePlate.ToString()));

            return newLicensePlate.ToString();
        }

        private string GetRandimCertificateNumber()
        {
            var certificateNumber = new StringBuilder();
            var random = new Random();

            do
            {
                certificateNumber.Clear();

                for (int i = 0; i < 10; i++)
                    certificateNumber.Append(random.Next(10));
            }
            while (App.DbContext.Certificates.Any(c => c.Number == certificateNumber.ToString()));

            return certificateNumber.ToString();
        }

        private void CreateSertificate()
        {
            // Подтверждение 
            bool accept = MessageHelper.ConfirmSave();
            if (!accept) return;

            // Формирование 
            InspectionService inspectionService = new(Inspection);
            inspectionService.CreateCertificate(CertificateNumber, NewLicensePlate);

            // Смена флажка о сохранении
            Saved = true;
            Close();
        }

        // Обработчики событий
        private void Exit_Click(object sender, RoutedEventArgs e) => MessageHelper.ConfirmExit(this);

        private void Add_Click(object sender, RoutedEventArgs e) => CreateSertificate();
    }
}
