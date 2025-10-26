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
    /// Логика взаимодействия для InfoCertificateDialog.xaml
    /// </summary>
    public partial class InfoCertificateDialog : Window
    {
        public InfoCertificateDialog(Certificate certificate)
        {
            InitializeComponent();

            DataContext = certificate;
        }

        private void Exit_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
