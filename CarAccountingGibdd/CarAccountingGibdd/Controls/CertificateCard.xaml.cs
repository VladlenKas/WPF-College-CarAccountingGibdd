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
    /// Логика взаимодействия для CertificateCard.xaml
    /// </summary>
    public partial class CertificateCard : UserControl
    {
        // Поля
        private Certificate _certificate;

        // Конструктор
        public CertificateCard(Certificate certificate)
        {
            InitializeComponent();

            _certificate = certificate;
            DataContext = certificate;
        }

        // Обработчики событий
        private void Card_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InfoCertificateDialog dialog = new(_certificate);
            ComponentsHelper.ShowDialogWindowDark(dialog);
        }

        private void GetInfo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            e.Handled = true; // Останавливаем всплытие события

            InfoCertificateDialog dialog = new(_certificate);
            ComponentsHelper.ShowDialogWindowDark(dialog);
        }
    }
}
