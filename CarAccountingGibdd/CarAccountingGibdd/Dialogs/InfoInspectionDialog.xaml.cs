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

            if (inspection.StatusId == 3) // Если заявка прошла осмотр
            {
                titleInfoDocumentTB.Text = "Номер сертификата:";
                infoDocumentTB.Text = inspection.Application.Certificates.First().ApplicationId.ToString();
            }
            else if (inspection.StatusId == 4 || inspection.StatusId == 5) // Если заявка не прошла осмотр
            {
                titleInfoDocumentTB.Text = "Номер документа о нарушениях:";
                infoDocumentTB.Text = $"Нарушения ТС по инспекции №{inspection.InspectionId}" ;
            }
        }

        private void ExitBTN_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
