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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CarAccountingGibdd.Controls
{
    /// <summary>
    /// Логика взаимодействия для SelectedViolationControl.xaml
    /// </summary>
    public partial class SelectedViolationControl : UserControl
    {
        // Свойства
        public event EventHandler<ViolationEventArgs> ViolationDeleteEvent;
        public Violation Violation { get; private set; }

        // Конструктор
        public SelectedViolationControl(Violation violation)
        {
            InitializeComponent();

            Violation = violation;
            DataContext = violation;
        }

        // Обработчики событий
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            ViolationDeleteEvent.Invoke(Violation, new ViolationEventArgs { Violation = this.Violation });
        }
    }
}
