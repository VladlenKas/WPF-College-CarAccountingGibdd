using CarAccountingGibdd.Classes;
using CarAccountingGibdd.Dialogs;
using CarAccountingGibdd.Model;
using MaterialDesignThemes.Wpf;
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
    /// Логика взаимодействия для ImageMiniatureCard.xaml
    /// </summary>
    public partial class ImageMiniatureCard : UserControl
    {
        // Событие 
        public event EventHandler<PhotosVehicleEventArgs> ImageDeleteEvent;

        // Свойства
        public PhotosVehicle PhotosVehicle { get; private set; }

        // Конструктор для редактирования
        public ImageMiniatureCard(PhotosVehicle photosVehicle, Window dialog)
        {
            InitializeComponent();

            imageVehicle.Source = photosVehicle.BitmapImage;
            PhotosVehicle = photosVehicle;
        }

        // Конструктор для просмотра
        public ImageMiniatureCard(PhotosVehicle photosVehicle)
        {
            InitializeComponent();

            deleteBTN.Visibility = Visibility.Hidden; // Скрываем кнопку удаления

            imageVehicle.Source = photosVehicle.BitmapImage;
            PhotosVehicle = photosVehicle;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            ImageDeleteEvent.Invoke(PhotosVehicle, new PhotosVehicleEventArgs { PhotosVehicle = this.PhotosVehicle });
        }

        private void ImageVehicle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OpenImageDialog dialog = new OpenImageDialog(PhotosVehicle);
            dialog.ShowDialog();
        }
    }
}
