using CarAccoutingGibdd.Model;
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

namespace CarAccoutingGibdd.Windows
{
    /// <summary>
    /// Логика взаимодействия для NavigationAdminWindow.xaml
    /// </summary>
    public partial class NavigationAdminWindow : Window
    {
        // Поля и свойства
        private GibddContext _dbContext;
        private Employee _thisEmpoyee;

        // Конструктор
        public NavigationAdminWindow(Employee employee)
        {
            InitializeComponent();
            Title = $"Меню Администратора. Сотрудник: {employee.Fullname}";

            _thisEmpoyee = employee;
        }
    }
}
