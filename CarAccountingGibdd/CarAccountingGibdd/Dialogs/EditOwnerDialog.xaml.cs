using CarAccountingGibdd.Classes.Services;
using CarAccountingGibdd.Classes;
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
using CarAccountingGibdd.Model;

namespace CarAccountingGibdd.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для EditOwnerDialog.xaml
    /// </summary>
    public partial class EditOwnerDialog : Window
    {
        // Свойства
        public bool Saved { get; private set; } // Флаг сохранения

        // Поля
        private Owner _owner;

        // Конструктор
        public EditOwnerDialog(Owner owner)
        {
            InitializeComponent();

            firstnameTB.Text = owner.Firstname;
            lastnameTB.Text = owner.Lastname;
            patronymicTB.Text = owner.Patronymic;
            dateTB.DateText = owner.Birthdate.ToString();
            emailTB.Text = owner.Email;
            phoneTB.PhoneNumber = owner.Phone;
            passportTB.Text = owner.Passport;
            addressTB.Text = owner.Address;

            _owner = owner;
        }

        // Методы
        private async void Edit()
        {
            // Получаем данные для добавления владельца
            string firstname = firstnameTB.Text;
            string lastname = lastnameTB.Text;
            string patronymic = patronymicTB.Text;
            string birthdate = dateTB.DateText;
            string email = emailTB.Text;
            string phone = phoneTB.PhoneNumber;
            string passport = passportTB.Text;
            string address = addressTB.Text;

            // Создаем экземпляр сервиса
            OwnerService service = new OwnerService(firstname, lastname, patronymic, birthdate, email, phone, passport, address);

            // Проверка
            bool notError = await service.CheckAsync(_owner);
            if (!notError) return;

            // Подтверждение
            bool accept = MessageHelper.ConfirmEdit();
            if (!accept) return;

            // Формирование
            service.Update(_owner);

            // Закрываем и сменяем флажок 
            Saved = true;
            Close();
        }

        // Обработчики событий
        private void Exit_Click(object sender, RoutedEventArgs e) => MessageHelper.ConfirmExit(this);

        private void Add_Click(object sender, RoutedEventArgs e) => Edit();
    }
}
