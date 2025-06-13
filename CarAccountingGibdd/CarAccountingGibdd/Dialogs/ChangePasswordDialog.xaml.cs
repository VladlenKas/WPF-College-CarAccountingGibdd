using CarAccountingGibdd.Classes.Services;
using CarAccountingGibdd.Classes;
using CarAccountingGibdd.Model;
using CarAccountingGibdd.Windows;
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
using System.Net.Mail;
using System.Net;

namespace CarAccountingGibdd.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для ChangePasswordDialog.xaml
    /// </summary>
    public partial class ChangePasswordDialog : Window
    {
        // Поля и свойства
        private string Email => emailTB.Text;
        private string newPassword => ComponentsHelper.GetPassword(PassPB, PassTB);

        // Конструктор
        public ChangePasswordDialog()
        {
            InitializeComponent();
            ApplicationService.HasStartedInspections(); // Проверка на просроченные записи
        }

        // Методы
        private Employee? FindEmployee(string email)
        {
            return App.DbContext.Employees.SingleOrDefault(r =>
                r.Email == email);
        }

        private void ChangePassword()
        {
            if (Email == string.Empty || newPassword == string.Empty)
            {
                MessageHelper.MessageNullFields();
                return;
            }

            var employee = FindEmployee(Email);

            if (employee == null)
            {
                // Если пользователь не найден
                MessageBox.Show("Пользователь с указанной электронной почтой не найден!",
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            else
            {
                var result = MessageBox.Show("Вы подтверждаете смену пароля?",
                                             "Подтверждение",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Отправляем пароль на почту
                    MailAddress from = new MailAddress("kasimovvladlen2006@yandex.ru", "Управление ГИБДД");
                    MailAddress to = new MailAddress(Email);
                    MailMessage message = new MailMessage(from, to);

                    message.Subject = "Восстановление пароля";
                    message.Body =
                        "Сброс пароля<br><br>" +
                        "Здравствуйте!<br><br>" +
                        "Ваш пароль был успешно сброшен по запросу восстановления доступа к сервису \"Управление ГИБДД\".<br><br>" +
                        $"Ваш e-mail: {Email}<br>" +
                        $"Ваш новый пароль: {newPassword}<br><br>" +
                        "Пожалуйста, используйте новый пароль для входа в систему.<br>" +
                        "Если вы не запрашивали сброс пароля, проигнорируйте это письмо или свяжитесь с поддержкой.<br><br>" +
                        "С уважением,<br>" +
                        "Служба поддержки ГИБДД";
                    message.IsBodyHtml = true;

                    SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 25) 
                    {
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential("kasimovvladlen2006@yandex.ru", "czwycjcedmqhebcd")
                    };
                    smtp.Send(message);

                    // Уведомляем пользователя
                    MessageBox.Show("Пароль успешно изменен!",
                                    "Успех",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);

                    // Меняем данные пароля
                    employee.Password = newPassword;
                    App.DbContext.Update(employee);
                    App.DbContext.SaveChanges();

                    this.Close();
                }
            }
        }

        // Обработчики событий
        private void Login_Click(object sender, RoutedEventArgs e) => ChangePassword();

        private void Exit_Click(object sender, RoutedEventArgs e) => MessageHelper.ConfirmExit(this);

        private void VisibilityPassword_Click(object sender, RoutedEventArgs e) => ComponentsHelper.ToggleVisibility(sender, PassPB, PassTB);
    }
}
