using CarAccountingGibdd.Classes;
using CarAccountingGibdd.Classes.Services;
using CarAccountingGibdd.Controls;
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
    /// Логика взаимодействия для AddViolationInspectionDialog.xaml
    /// </summary>
    public partial class AddViolationInspectionDialog : Window
    {
        // Свойства 
        public bool Saved { get; private set; } // Флаг сохранения

        // Поля
        private Inspection _inspection;
        private List<Violation> _violationsList = new();

        // Конструктор
        public AddViolationInspectionDialog(Inspection inspection)
        {
            InitializeComponent();

            violationsATB.ItemsSource = App.DbContext.Violations;
            violationsATB.ItemSelected += ViolationsATB_ItemSelected;

            _inspection = inspection;
            DataContext = _inspection;
        }

        // Методы
        private void CreateViolationInspection()
        {
            // Проверка
            if (_violationsList.Count < 0)
            {
                MessageHelper.MessageNullViolations();
                return;
            }

            // Подтверждение 
            bool accept = MessageHelper.ConfirmSave();
            if (!accept) return;

            // Формирование 
            InspectionService inspectionService = new(_inspection);
            inspectionService.CreateViolationsInspection(_violationsList);

            // Смена флажка о сохранении
            Saved = true;
            Close();
        }

        // Обработчики событий
        private void Exit_Click(object sender, RoutedEventArgs e) => MessageHelper.ConfirmExit(this);

        private void Add_Click(object sender, RoutedEventArgs e) => CreateViolationInspection();

        private void ViolationsATB_ItemSelected(object obj)
        {
            if (obj != null)
            {
                Violation selectedViolation = (Violation)violationsATB.SelectedItem;

                if (_violationsList.All(itemList => itemList.ViolationId != selectedViolation.ViolationId))
                {
                    _violationsList.Add(selectedViolation);

                    var control = new SelectedViolationControl(selectedViolation);
                    control.ViolationDeleteEvent += DeleteViolation;
                    violationsIC.Items.Add(control);
                }

                violationsATB.Text = string.Empty; 
            }
        }

        private void DeleteViolation(object sender, ViolationEventArgs e)
        {
            Violation violation = (Violation)sender;
            int index = _violationsList.FindIndex(0, itemList => itemList.ViolationId == violation.ViolationId);

            _violationsList.RemoveAt(index);
            violationsIC.Items.RemoveAt(index);
        }
    }
}
