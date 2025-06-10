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
    /// Логика взаимодействия для AddVehicleDialog.xaml
    /// </summary>
    public partial class AddVehicleDialog : Window
    {
        // Поля и свойства
        public bool Saved { get; private set; } // Флаг сохранения

        // Конструктор
        public AddVehicleDialog()
        {
            InitializeComponent();
        }
    }
}
