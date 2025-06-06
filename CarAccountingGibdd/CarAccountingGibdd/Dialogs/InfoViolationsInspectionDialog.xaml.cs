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
    /// Логика взаимодействия для InfoViolationsInspectionDialog.xaml
    /// </summary>
    public partial class InfoViolationsInspectionDialog : Window
    {
        // Свойства
        public string ViolationsList { get; set; }
        public Inspection Inspection { get; set; }

        public InfoViolationsInspectionDialog(IGrouping<int, ViolationInspection> violationsInspection)
        {
            InitializeComponent();

            Inspection inspection = violationsInspection.First().Inspection;

            Inspection = inspection;
            DataContext = this;

            var violations = new StringBuilder();
            int count = 1;
            foreach (var item in violationsInspection)
            {
                violations.AppendLine($"{count++}: {item.Violation.Description}");
            }
            ViolationsList = violations.ToString();
        }

        private void ExitBTN_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
