using CarAccountingGibdd.Classes;
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
    /// Логика взаимодействия для OpenImageDialog.xaml
    /// </summary>
    public partial class OpenImageDialog : Window
    {
        // Конструктор
        public OpenImageDialog(PhotosVehicle photosVehicle)
        {
            InitializeComponent();
            imageVehicle.Source = photosVehicle.BitmapImage;
        }

        // Обработчики событий
        private void Exit_Click(object sender, RoutedEventArgs e) => Close();
    }
}
