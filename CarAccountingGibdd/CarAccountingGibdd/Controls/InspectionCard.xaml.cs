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
        public event EventHandler<InspectionEventArgs> InspectionToAcceptEvent;
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
            int post = _employee.PostId;
            int status = _inspection.InspectionStatusId;

            // Учитываем, что кнопки скрыты

            // Логика для оператора
            if (post == 3) // оператор
            {
                infoBTN.Visibility = Visibility.Visible;
            }
            else // для инспектора
            {
                if (status == 1) // осмотр не начат
                {
                    startInspectionBTNS.Visibility = Visibility.Visible;
                }
                else if (status == 2) // в процессе
                {
                    endInspectionBTN.Visibility = Visibility.Visible;
                }
                else // все остальные статусы
                {
                    infoBTN.Visibility = Visibility.Visible;
                }
            }

            // Прозрачность зависит от статуса осмотра
            if (status == 3) // например, статус "завершён" или другой, когда нужна прозрачность
            {
                this.Opacity = 0.5;
            }
            else
            {
                this.Opacity = 1.0;
            }

            // Закрашиваем Border в красный, если опоздание
            DateTime datetimePlanned = _inspection.DatetimePlanned.AddHours(2);
            if (DateTime.Now > datetimePlanned)
            {
                importantPushTB.Visibility = Visibility.Visible;
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

            InspectionToAcceptEvent.Invoke(this, new InspectionEventArgs { Inspection = this.Inspection });
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

            InspectionToAcceptEvent.Invoke(this, new InspectionEventArgs { Inspection = this.Inspection });
        }

        private void EndInspection_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            e.Handled = true; // Останавливаем всплытие события

            bool? result = MessageHelper.GetResultInspection();
            bool saved = false;

            if (result == true)
            {
                SelectRegionDialog dialog = new(_inspection);
                ComponentsHelper.ShowDialogWindowDark(dialog);
            }
            else if (result == false)
            {
                AddViolationInspectionDialog dialog = new(_inspection);
                ComponentsHelper.ShowDialogWindowDark(dialog);
            }

            if (saved) return;
            InspectionToAcceptEvent.Invoke(this, new InspectionEventArgs { Inspection = this.Inspection });
        }
    }
}
