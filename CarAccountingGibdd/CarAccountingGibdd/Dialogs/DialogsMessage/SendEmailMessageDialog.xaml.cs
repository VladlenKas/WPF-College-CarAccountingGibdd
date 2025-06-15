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

namespace CarAccountingGibdd.Dialogs.DialogsMessage
{
    /// <summary>
    /// Логика взаимодействия для SendEmailMessageDialog.xaml
    /// </summary>
    public partial class SendEmailMessageDialog : Window
    {
        public SendEmailMessageDialog()
        {
            InitializeComponent();
        }

        // Методы
        public void VisibleClose()
        {
            loadGif.Visibility = Visibility.Collapsed; // Убираем гифку
            exitBTN.Visibility = Visibility.Visible; // Показываем кнопку для выхода
            infoTB.Text = "Сообщение отправлено успешно!";
        }

        private void Exit_Click(object sender, RoutedEventArgs e) => Close();
    }
}
