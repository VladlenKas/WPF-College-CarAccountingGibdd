using CarAccountingGibdd.Classes.Services;
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
using static System.Net.Mime.MediaTypeNames;

namespace CarAccountingGibdd.Controls
{
    /// <summary>
    /// Логика взаимодействия для InspectionCard.xaml
    /// </summary>
    public partial class InspectionCard : UserControl
    {
        // Свойства
        public event EventHandler<InspectionEventArgs> InspectionToAccept;
        public Inspection Inspection { get; private set; }

        // Поля
        private Inspection _inspection;
        private Employee _employee;

        // Конструктор
        public InspectionCard(Inspection inspection, Employee employee)
        {
            InitializeComponent();

            _inspection = inspection;
            _employee = employee;

            DifferentiationFunctionality();
            LoadCardData();
        }

        // Свойства
        private void DifferentiationFunctionality()
        {
            // Разграничиваем функционал по статусам
            int status = _inspection.StatusId;

            if (status == 1) // если осмотр не неачат
            {
                startInspectionBTNS.Visibility = System.Windows.Visibility.Visible;
            }
            else if (status == 2) // в процессе
            {
                endInspectionBTN.Visibility = System.Windows.Visibility.Visible;
            }
            else // все остальные случаи
            {
                infoBTN.Visibility = System.Windows.Visibility.Visible;
                Opacity = 0.5;
            }
        }

        private void LoadCardData()
        {
            App.DbContext.Attach(_inspection);
            DataContext = _inspection;
        }

        // Обработчики событий
        private void Card_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InfoInspectionDialog dialog = new(_inspection);
            ComponentsHelper.ShowDialogWindowDark(dialog);
        }

        private void GetInfo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            e.Handled = true; // Останавливаем всплытие события

            InfoInspectionDialog dialog = new(_inspection);
            ComponentsHelper.ShowDialogWindowDark(dialog);
        }

        private void Reject_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            e.Handled = true; // Останавливаем всплытие события


            bool accept = MessageHelper.ConfirmRejectInspection();
            if (accept) 
            {
                InspectionService inspectionService = new(_inspection);
                inspectionService.Reject();
            }

            InspectionToAccept.Invoke(this, new InspectionEventArgs { Inspection = this.Inspection });
        }

        private void StartInspection_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            e.Handled = true; // Останавливаем всплытие события

            bool accept = MessageHelper.ConfirmStartInspection();
            if (accept)
            {
                InspectionService inspectionService = new(_inspection);
                inspectionService.StartInspection();
            }

            InspectionToAccept.Invoke(this, new InspectionEventArgs { Inspection = this.Inspection });
        }

        private void EndInspection_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            e.Handled = true; // Останавливаем всплытие события

            bool result = MessageHelper.GetResultInspection();
            bool saved = false;

            if (result)
            {
                AddSertificateDialog dialog = new(_inspection);
                ComponentsHelper.ShowDialogWindowDark(dialog);
            }
            else if (!result)
            {
                /*AcceptApplicationDialog dialog = new(_inspection, _employee);
                ComponentsHelper.ShowDialogWindowDark(dialog);*/
            }

            if (saved) return;
            InspectionToAccept.Invoke(this, new InspectionEventArgs { Inspection = this.Inspection });
        }
    }
}
