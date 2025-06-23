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
    /// Логика взаимодействия для InfoInspectionDialog.xaml
    /// </summary>
    public partial class InfoInspectionDialog : Window
    {
        // Конструктор
        public InfoInspectionDialog(Inspection inspection)
        {
            InitializeComponent();
            DataContext = inspection;

            if (inspection.StatusId == 3) // Если инспекция прошла осмотр
            {
                titleInfoDocumentTB.Text = "Номер свидетельства:";
                infoDocumentTB.Text = $"{inspection.Application.Certificates.Single().CertificateId}";
            }
            else if (inspection.StatusId == 4 || inspection.StatusId == 5) // Если инспекция не прошла осмотр
            {
                titleInfoDocumentTB.Text = "Номер отчета о выявленных нарушениях:";
                infoDocumentTB.Text = $"Отчет о нарушениях №{inspection.InspectionId}" ;
            }
        }

        private void ExitBTN_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
