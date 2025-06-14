using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CarAccountingGibdd.Classes
{
    public static class MessageHelper
    {
        // Область предупреждений | уведомлений

        private static void ShowWarning(string message) => 
            MessageBox.Show(message, "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);

        public static void MessageUniversal(string message) =>
            ShowWarning(message);

        public static void MessageNullFields() =>
            ShowWarning("Заполните все поля!");

        public static void MessageNullFilepath() =>
            ShowWarning("Выберите путь для сохранения документа! Для этого нажмите на кнопку " +
                "«Выбрать путь к документу»");

        public static void MessageShortFio() =>
            ShowWarning("Фамилия имя и отчество должны содержать минимум 3 символа!");

        public static void MessageNullDate() =>
            ShowWarning("Дата рождения должна соответствовать формату «dd.MM.yyyy»!");

        public static void MessageInappropriateAge() =>
            ShowWarning("Возраст должен быть в промежутке от 18 до 85 лет включительно!");

        public static void MessageShortPhone() =>
            ShowWarning("Номер телефона должен содержать 11 цифр!");

        public static void MessageShortPassport() =>
            ShowWarning("Серия и номера паспорта должны содержать 10 цифр!");

        public static void MessageShortAddress() =>
            ShowWarning("Адрес должен содержать минимум 10 символов!");

        public static void MessageDuplicatePhone() =>
            ShowWarning("Такой номер телефона уже существует! Введите другой");

        public static void MessageDuplicatePassport() =>
            ShowWarning("Такие серия и номер паспорта уже существуют! Введите другие");
        
        public static void MessageDuplicateVin() =>
            ShowWarning("Такой ВИН уже существует у другого ТС! Введите другой");
        
        public static void MessageDuplicateLicensePlate() =>
            ShowWarning("Такой номерной знак уже существует у другого ТС! Введите другой");

        public static void MessageDuplicateEmail() =>
            ShowWarning("Такая электронная почта уже существует! Введите другую");

        public static void MessageNullViolations() =>
            ShowWarning("Выберите минимум 1 нарушение!");

        public static void MessageNullCost() =>
            ShowWarning("Данных средств недостаточно для оплаты гос. пошлины!\nПожалуйста, укажите другую сумму");

        public static void MessageActiveApplication() =>
            ShowWarning("В настоящее время выбранное транспортное средство уже имеет действующую заявку. " +
                "Гражданин не может подавать сразу несколько заявок на одно транспортное средство");

        public static void MessageCerrentSertificate() =>
            ShowWarning("Данный владелец уже имеет действующее свидетельство о регистрации ТС. " +
                "Нельзя выдавать несколько сертификатов одному владельцу");

        public static void MessageCerrentOwner() =>
            ShowWarning("Данное транспортное средство уже имеет владельца и свидетельство о регистрации ТС. Подать заявку невозможно");

        public static void MessageInvalidLicensePlate() =>
            ShowWarning("Неверный формат номерного знака!\nНомерной знак должен содержать ровно 6 символов, " +
                "состоящих из заглавных букв кириллицы и цифр\nПример корректного номерного знака: А671КВ");
        
        public static void MessageInvalidVin() =>
            ShowWarning("Неверный формат VIN-кода!\nVIN должен содержать ровно 17 символов, " +
                "состоящих из заглавных латинских букв(кроме I, O и Q) и цифр\nПример корректного VIN: 1HGCM82633A004352");

        public static void MessageInvalidEmail() =>
            ShowWarning("Неверный формат электронной почты!\nЭлектронная почта должна содержать символы «@», «.» и иметь домен от " +
                "2-х до 4-х символов" +
                "\nПример корректной электронной почты: employee2025@gmail.com");

        public static void MessageNotChanges() =>
            MessageBox.Show("Вы не внесли изменений", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

        // Область подтверждений
        private static bool ConfirmAction(string confirmationMessage, string successMessage)
        {
            string title = "Подтверждение";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;

            var result = MessageBox.Show(confirmationMessage, title, button, icon);
            if (result == MessageBoxResult.Yes)
            {
                MessageBox.Show(successMessage, "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            return false;
        }

        public static bool ConfirmSave() =>
            ConfirmAction("Вы уверены, что все поля заполнены верно?", "Добавление прошло успешно");

        public static bool ConfirmEdit() =>
            ConfirmAction("Вы уверены, что все поля заполнены верно?", "Изменение прошло успешно");

        public static bool ConfirmSaveApplication() =>
            ConfirmAction(
                "Вы уверены, что заполнили все поля верно? После одобрения заявки внести изменения будет НЕВОЗМОЖНО!",
                "Формирование заявки прошло успешно");

        public static bool ConfirmDeleteOwner() =>
            ConfirmAction(
                "Вы действительно хотите удалить этого владельца?\n" +
                "Внимание! При удалении владельца, все связанные свидетельства станут недействительными",
                "Удаление прошло успешно");
        
        public static bool ConfirmDeleteViolation() =>
            ConfirmAction(
                "Вы действительно хотите удалить это нарушение?",
                "Удаление прошло успешно");
        
        public static bool ConfirmDeleteDepartment() =>
            ConfirmAction(
                "Вы действительно хотите удалить этот департамент?",
                "Удаление прошло успешно");
        
        public static bool ConfirmDeleteEmployee() =>
            ConfirmAction(
                "Вы действительно хотите удалить этого сотрудника?",
                "Удаление прошло успешно");
        
        public static bool ConfirmDeleteVehicle() =>
            ConfirmAction(
                "Вы действительно хотите удалить это транспортное средство?\n" +
                "Внимание! При удалении ТС, все связанные свидетельства станут недействительными",
                "Удаление прошло успешно");

        public static bool ConfirmEditApplication() =>
            ConfirmAction(
                "Вы уверены, что заполнили все поля верно? После одобрения заявки внести изменения будет НЕВОЗМОЖНО!",
                "Редактирование заявки прошло успешно");

        public static bool ConfirmRejectApplication() =>
            ConfirmAction(
                "Вы уверены, что хотите отклонить заявку? Данное действие отменить НЕВОЗМОЖНО!",
                "Заявка октлонена успешно");

        public static bool ConfirmApplication() =>
            ConfirmAction(
                "Вы уверены, что хотите подтвердить заявку? После данного действия заявка будет доступна для проведения инспекции ТС!",
                "Заявка подтверждена успешно");

        public static bool ConfirmAcceptApplication() =>
            ConfirmAction(
                "Вы уверены, что хотите принять заявку? После данного действия будут запланированы дата и время для проведения инспекции, которую необходимо будет провести! В случае неявки гражданина, заявка и инспекция будут автоматически анулированы",
                "Заявка на проведение инспекции принята успешно");

        public static bool ConfirmRejectInspection() =>
            ConfirmAction(
                "Вы уверены, что хотите отменить запланированный осмотр? Данное действие отменить НЕВОЗМОЖНО!",
                "Осмотр отменен успешно");

        public static bool ConfirmStartInspection() =>
            ConfirmAction(
                "Вы уверены, что хотите начать проведение осмотра?",
                "Проведение инспекции начато успешно");
        
        public static bool ConfirmDetachOwner() =>
            ConfirmAction(
                "Вы уверены, что хотите отвязать ТС от текущего владельца? " +
                "Раннее действительное свидетельство станет неактуальным!",
                "ТС успешно отвязан от текущего владельца!");

        // Другое
        public static void ConfirmExit(Window window)
        {
            var resultChanged = MessageBox.Show("Вы действительно хотите выйти?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resultChanged == MessageBoxResult.Yes)
            {
                window.Close();
            }
        }

        public static bool GetResultInspection()
        {
            var resultChanged = MessageBox.Show("Инспекция прошла успешно?",
                "Вопрос",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question);

            if (resultChanged == MessageBoxResult.Yes)
            {
                return true;
            }
            else
            {
                return false;
            }
        } 
    }
}
