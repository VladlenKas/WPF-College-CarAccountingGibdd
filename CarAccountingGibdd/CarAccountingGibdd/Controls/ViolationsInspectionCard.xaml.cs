using CarAccountingGibdd.Classes;
using CarAccountingGibdd.Dialogs;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CarAccountingGibdd.Controls
{
    /// <summary>
    /// Логика взаимодействия для ViolationsInspectionCard.xaml
    /// </summary>
    public partial class ViolationsInspectionCard : UserControl
    {
        // Свойства
        public string CountViolations { get; set; }
        public Inspection Inspection { get; set; }

        // Поля
        private IGrouping<int, ViolationInspection> _violationsInspection;
        private Employee _employee;

        // Конструктор
        public ViolationsInspectionCard(IGrouping<int, ViolationInspection> violationsInspection, Employee employee)
        {
            InitializeComponent();

            // Даем возможность инспектору печатать документ
            if (employee.PostId == 2)
            {
                saveDocumentBTN.Visibility = Visibility.Visible;
                _employee = employee;
            }

            _violationsInspection = violationsInspection;
            Inspection inspection = violationsInspection.First().Inspection;

            Inspection = inspection;
            CountViolations = violationsInspection.Count().ToString();
            DataContext = this;
        }

        // Обработчики событий
        private void Card_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InfoViolationsInspectionDialog dialog = new(_violationsInspection);
            ComponentsHelper.ShowDialogWindowDark(dialog);
        }

        private void GetInfo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            e.Handled = true; // Останавливаем всплытие события

            InfoViolationsInspectionDialog dialog = new(_violationsInspection);
            ComponentsHelper.ShowDialogWindowDark(dialog);
        }

        private void SaveDocument_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            e.Handled = true; // Останавливаем всплытие события

            SaveDocumentDialog dialog = new(_violationsInspection, _employee);
            ComponentsHelper.ShowDialogWindowDark(dialog);
        }
    }
}
