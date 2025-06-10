using CarAccountingGibdd.Classes;
using CarAccountingGibdd.Classes.Services;
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
    /// Логика взаимодействия для AddOwnerDialog.xaml
    /// </summary>
    public partial class AddOwnerDialog : Window
    {
        // Поля и свойства
        public bool Saved { get; private set; } // Флаг сохранения
        
        // Конструктор
        public AddOwnerDialog()
        {
            InitializeComponent();
        }

        // Методы
        private void AddOwner()
        {
            // Получаем данные для добавления владельца
            string firstname = firstnameTB.Text;
            string lastname = lastnameTB.Text;
            string patronymic = patronymicTB.Text;
            string birthdate = dateTB.DateText;
            string phone = phoneTB.PhoneNumber;
            string passport = passportTB.Text;
            string address = addressTB.Text;

            // Создаем экземпляр сервиса
            OwnerService service = new OwnerService(firstname, lastname, patronymic, birthdate, phone, passport, address);

            // Проверка
            bool notError = service.Check();
            if (!notError) return;

            // Подтверждение
            bool accept = MessageHelper.ConfirmSave();
            if (!accept) return;

            // Формирование
            service.Add();

            // Закрываем и сменяем флажок 
            Saved = true;
            Close();
        }

        // Обработчики событий
        private void Exit_Click(object sender, RoutedEventArgs e) => MessageHelper.ConfirmExit(this);

        private void Add_Click(object sender, RoutedEventArgs e) => AddOwner();
    }
}
