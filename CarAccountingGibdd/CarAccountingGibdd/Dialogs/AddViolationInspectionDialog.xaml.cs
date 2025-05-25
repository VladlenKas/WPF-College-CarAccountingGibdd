using CarAccountingGibdd.Classes;
using CarAccountingGibdd.Classes.Services;
using CarAccountingGibdd.Controls;
using CarAccountingGibdd.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
using Microsoft.Win32;

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
        private string _filepath;

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
        private async Task SendMessageAsync()
        {
            string? email = _inspection.Application.Owner.Email;
            if (email == null) return;

            MessageBox.Show("Письмо формируется. Через несколько секунд вам придет уведомление о его завершении!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

            MailAddress from = new MailAddress("caraccounting.gibdd@yandex.ru", "Управление ГИБДД");
            MailAddress to = new MailAddress(email);
            MailMessage message = new MailMessage(from, to);

            message.Subject = "Уведомление о результатах проверки ТС";
            message.Body =
                $"Здравствуйте!<br><br>" +
                $"Ваше транспортное средство не прошло инспекцию.<br>" +
                $"Причина отказа указана в отчете о выявленных нарушениях, который прикреплен ниже к данному письму.<br><br>" +
                "После исправления нарушений подайте заявку на повторную проверку.<br><br>" +
                "С уважением,<br>" +
                "Служба поддержки ГИБДД";
            message.IsBodyHtml = true;

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
                Credentials = new NetworkCredential("caraccounting.gibdd@yandex.ru", "byksxvejlsanrjsb")
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

        private bool SaveFilePath()
        {
            SaveFileDialog saveFileDialog = null;

            // Выбор пути
            saveFileDialog = new SaveFileDialog()
            {
                Filter = "Pdf Files|*.pdf*",
                Title = "Сохранить PDF документ",
                FileName = $"Отчёт о выявленных нарушениях №{_inspection.InspectionId}"
            };

            // Если пользователь выбрал путь для сохранения чека
            if (saveFileDialog.ShowDialog() == true)
            {
                _filepath = $"{saveFileDialog.FileName}.pdf"; // Путь для открытия файла
                return true;
            }

            return false;
        }

        private async Task Create()
        {
            // Проверка
            if (_violationsList.Count <= 0)
            {
                MessageHelper.MessageNullViolations();
                return;
            }

            // Открытие окна и выбор пути к документу
            bool saved = SaveFilePath();
            if (!saved) return;

            // Подтверждение 
            bool accept = MessageHelper.ConfirmSave();
            if (!accept) return;

            // Формирование 
            InspectionService inspectionService = new(_inspection);
            inspectionService.CreateViolationsInspection(_violationsList);

            // Формирование документа 
            DocumentService.GenerateViolationsReport(_filepath, _inspection.InspectionId, _inspection.Inspector.Fullname);

            // Смена флажка о сохранении
            Saved = true;
            Close();

            // Отправляем письмо в фоне
            _ = SendMessageAsync();
        }

        // Обработчики событий
        private void Exit_Click(object sender, RoutedEventArgs e) => MessageHelper.ConfirmExit(this);

        private async void Add_Click(object sender, RoutedEventArgs e) => await Create();

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
