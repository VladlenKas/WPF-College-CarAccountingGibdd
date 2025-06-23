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
using System.Windows.Shapes;

namespace CarAccountingGibdd.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для InfoApplicationDialog.xaml
    /// </summary>
    public partial class InfoApplicationDialog : Window
    {
        // Конструктор
        public InfoApplicationDialog(Model.Application application)
        {
            InitializeComponent();
            DataContext = application;

            if (application.ApplicationStatusId == 6) // Если заявка отклонена
            {
                datetimeConfirmTB.Text = "Дата и время отклонения:";
            }

            var inspection = application.Inspections.SingleOrDefault();

            if (inspection == null)
            {
                return;
            }

            if (inspection.StatusId == 3) // Если инспекция прошла осмотр
            {
                titleInfoDocumentTB.Text = "Номер сертификата:";
                infoDocumentTB.Text = $"№{inspection.Application.Certificates.Single().CertificateId}";
            }
            else if (inspection.StatusId == 4 || inspection.StatusId == 5) // Если инспекция не прошла осмотр
            {
                titleInfoDocumentTB.Text = "Номер документа о нарушениях:";
                infoDocumentTB.Text = $"Отчёт о выявленных нарушениях №{inspection.InspectionId}";
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
