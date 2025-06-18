using CarAccountingGibdd.Classes;
using CarAccountingGibdd.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CarAccountingGibdd.Classes.Services;
using System.Net.Mail;
using System.Net;
using Microsoft.Win32;
using CarAccountingGibdd.Dialogs.DialogsMessage;

namespace CarAccountingGibdd.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для AddSertificateDialog.xaml
    /// </summary>
    public partial class AddCertificateDialog : Window
    {
        // Свойства для контекста
        public string NewLicensePlate { get; set; }
        public int CertificateId { get; set; }
        public string CertificateNumber { get; set; }
        public DateTime DatetimeIssue { get; set; }
        public Inspection Inspection { get; set; }
        
        // Свойства
        public bool Saved { get; private set; } // Флаг сохранения

        // Поля
        private string _filepath;

        // Конструктор
        public AddCertificateDialog(Inspection inspection)
        {
            InitializeComponent();

            Inspection = inspection;
            NewLicensePlate = GetNewLicensePlate();
            CertificateId = GetCertificateId();
            CertificateNumber = GetRandomCertificateNumber();
            DatetimeIssue = DateTime.Now;

            DataContext = this;
        }

        // Методы
        private int GetCertificateId()
        {
            if (App.DbContext.Certificates.Count() == 0)
            {
                return 1;
            }
            else
            {
                int lastId = App.DbContext.Certificates.OrderBy(i => i.CertificateId).Last().CertificateId;
                return lastId + 1;
            }
        }

        int GenerateRegionCode(Random random, List<(int Start, int End)> ranges)
        {
            var range = ranges[random.Next(ranges.Count)];
            return random.Next(range.Start, range.End + 1);
        }

        private string GetNewLicensePlate()
        {
            var newLicensePlate = new StringBuilder();
            var random = new Random();
            const string letters = "АБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЭЮЯ";

            var validRanges = new List<(int Start, int End)>
            {
                (1, 99),
                (102, 113),
                (116, 116),
                (121, 126),
                (134, 138),
                (142, 147),
                (150, 159),
                (161, 164),
                (173, 178),
                (186, 199),
                (702, 716)
            };

            do
            {
                newLicensePlate.Clear();

                newLicensePlate.Append(
                    $"{letters[random.Next(letters.Length)]}" +
                    random.Next(10) +
                    random.Next(10) +
                    random.Next(10) +
                    $"{letters[random.Next(letters.Length)]}" +
                    $"{letters[random.Next(letters.Length)]}"
                );

                int code = GenerateRegionCode(random, validRanges);
                string regionCode = code < 100 ? code.ToString("D2") : code.ToString();

                newLicensePlate.Append(regionCode);

            } 
            while (App.DbContext.Certificates.Any(c => c.Number == newLicensePlate.ToString()));

            return newLicensePlate.ToString();
        }

        private string GetRandomCertificateNumber()
        {
            var certificateNumber = new StringBuilder();
            var random = new Random();

            do
            {
                certificateNumber.Clear();

                for (int i = 0; i < 10; i++)
                    certificateNumber.Append(random.Next(10));
            }
            while (App.DbContext.Certificates.Any(c => c.Number == certificateNumber.ToString()));

            return certificateNumber.ToString();
        }

        private bool SaveFilePath()
        {
            SaveFileDialog saveFileDialog = null;

            // Выбор пути
            saveFileDialog = new SaveFileDialog()
            {
                Filter = "Pdf Files|*.pdf*",
                Title = "Сохранить PDF документ",
                FileName = $"Свидетельство о регистрации транспортного средства №{CertificateId}"
            };

            // Если пользователь выбрал путь для сохранения чека
            if (saveFileDialog.ShowDialog() == true)
            {
                _filepath = $"{saveFileDialog.FileName}.pdf"; // Путь для открытия файла
                return true;
            }

            return false;
        }

        private async Task SendMessageAsync()
        {
            string? email = Inspection.Application.Owner.Email;
            if (email == null) return;

            MessageBox.Show("Письмо формируется. Через несколько секунд вам придет уведомление о его завершении!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
           
            MailAddress from = new MailAddress("kasimovvladlen2006@yandex.ru", "Управление ГИБДД");
            MailAddress to = new MailAddress(email);
            MailMessage message = new MailMessage(from, to);

            message.Subject = "Уведомление о прохождении проверки ТС и присвоении нового регистрационного номера";
            message.Body =
                $"Здравствуйте!<br><br>" +
                $"Ваше транспортное средство успешно прошло инспекцию.<br>" +
                $"Теперь ему присвоен новый регистрационный номер: <b>{NewLicensePlate}</b>.<br><br>" +
                $"В приложении вы можете найти документ, подтверждающий это.<br><br>" +
                "Если у вас возникнут вопросы, пожалуйста, свяжитесь с нашей службой поддержки.<br><br>" +
                "С уважением,<br>" +
                "Служба поддержки ГИБДД";
            message.IsBodyHtml = true;

            // Путь к документу с подтверждением (PDF, изображение и т.п.)
            string filePath = _filepath; // замените на актуальный путь

            Attachment attachment = new Attachment(filePath);
            message.Attachments.Add(attachment);

            SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 25)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("kasimovvladlen2006@yandex.ru", "czwycjcedmqhebcd")
            };

            await smtp.SendMailAsync(message);
            try
            {
                await smtp.SendMailAsync(message);
                // Можно показать уведомление об успехе
                MessageBox.Show("Письмо успешно отправлено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                // Можно показать уведомление об ошибке
                MessageBox.Show("Ошибка при отправке письма: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                attachment.Dispose();
            }
        }

        private async Task CreateSertificate()
        {
            // Открытие окна и выбор пути к документу
            bool saved = SaveFilePath();
            if (!saved) return;

            // Подтверждение 
            bool accept = MessageHelper.ConfirmSave();
            if (!accept) return;

            // Формирование 
            InspectionService inspectionService = new(Inspection);
            Certificate certificate = inspectionService.CreateCertificate(CertificateNumber, NewLicensePlate);

            // Формирование документа 
            DocumentService.GenerateCertificateReport(_filepath, certificate, Inspection.Inspector.Fullname);

            // Смена флажка о сохранении
            Saved = true;
            Close();

            // Отправляем письмо в фоне
            _ = SendMessageAsync();
        }

        // Обработчики событий
        private void Exit_Click(object sender, RoutedEventArgs e) => MessageHelper.ConfirmExit(this);

        private async void Add_Click(object sender, RoutedEventArgs e) => await CreateSertificate();
    }
}
