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
    /// Логика взаимодействия для InfoOwnerDialog.xaml
    /// </summary>
    public partial class InfoOwnerDialog : Window
    {
        private int counter = 1;
        public string SequenceNumberVehicle
        {
            get
            {
                if (Owner.VehiclesList.Length > 0)
                    return $"{counter++}.";
                else
                    return string.Empty;
            }
        }

        public new Owner Owner { get; set; }

        public InfoOwnerDialog(Owner owner)
        {
            InitializeComponent();

            Owner = owner;
            DataContext = this;
        }
        private void ExitBTN_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
