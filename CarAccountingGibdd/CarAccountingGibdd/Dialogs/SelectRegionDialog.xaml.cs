using CarAccountingGibdd.Classes;
using CarAccountingGibdd.Model;
using ControlzEx.Standard;
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
    /// Логика взаимодействия для SelectRegionDialog.xaml
    /// </summary>
    public partial class SelectRegionDialog : Window
    {
        // Поля
        private Inspection _inspection;
        private string _region;

        // Свойства
        public bool Saved { get; private set; } // Флаг сохранения

        public SelectRegionDialog(Inspection inspection)
        {
            InitializeComponent();
            _inspection = inspection;

            regionTB.ItemsSource = FillList();
        }

        // Методы
        private List<LicensePlateRegion> FillList()
        {
            var validRanges = new List<(int Start, int End)>
            {
                (1, 99),
                (102, 113),
                (116, 116),
                (121, 126),
                (134, 138),
                (142, 147),
                (150, 159),
                (161, 164),
                (173, 178),
                (186, 199),
                (702, 716)
            };

            var regions = new List<LicensePlateRegion>();

            foreach (var (start, end) in validRanges)
            {
                for (int i = start; i <= end; i++)
                {
                    regions.Add(new LicensePlateRegion { Number = i.ToString("D2") });
                }
            }

            return regions;
        }

        private bool Check()
        {
            if (regionTB.SelectedItem == null)
            {
                MessageHelper.MessageNullFields();
                return false;
            }
            else
            {
                _region = ((LicensePlateRegion)regionTB.SelectedItem).Number;
                return true;
            }
        }

        // Обработчики событий
        private void Exit_Click(object sender, RoutedEventArgs e) => MessageHelper.ConfirmExit(this);

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            bool result = Check();
            if (!result) return;

            Hide();

            AddCertificateDialog addCertificateDialog = new AddCertificateDialog(_inspection, _region);
            addCertificateDialog.ShowDialog();

            if (addCertificateDialog.Saved == true)
            {
                Saved = true;
                Close();
            }
            else
            {
                ComponentsHelper.ShowDialogWindowDark(this);
            }
        }

    }

    class LicensePlateRegion
    {
        public string Number { get; set; } = null!;
    }
}
