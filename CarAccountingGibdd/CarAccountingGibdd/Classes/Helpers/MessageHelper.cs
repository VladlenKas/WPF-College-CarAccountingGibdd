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
        #region Предупреждения | уведомления
        // Предупреждение о пустых полях
        public static void MessageNullFields()
        {
            MessageBox.Show($"Заполните все поля!",
                "Предупреждение",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }

        // Предупреждение о пустой или нулевой цене
        public static void MessageNullCost()
        {
            MessageBox.Show($"Данных средств недостаточно для оплаты гос. пошлины!" +
                $"\nПожалуйста, укажите другую сумму",
                "Предупреждение",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }

        // Предупреждение о пустых полях
        public static void MessageActiveApplication()
        {
            MessageBox.Show($"В настоящее время выбранное транспортное средство уже имеет" +
                $" действующую заявку. Гражданин не может подавать сразу несколько" +
                $" заявок на одно транспортное средство",
                "Предупреждение",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }

        // Предупреждение о пустых полях
        public static void MessageCerrentSertificate()
        {
            MessageBox.Show($"Данный владелец уже имеет действующее сведетельство" +
                $" о регистрации ТС. Нельзя выдавать несколько сертификатов одному владельцу",
                "Предупреждение",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }

        // Предупреждение о пустых полях
        public static void MessageCerrentOwner()
        {
            MessageBox.Show($"Данное транспортное средство уже имеет владельца и" +
                $" сведетельство о регистрации ТС. Подать заявку невозможно",
                "Предупреждение",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }

        // Сообщение о том, что изменения не были внесены
        public static void MessageNotChanges()
        {
            MessageBox.Show($"Вы не внесли изменений",
                "Уведомление",
                MessageBoxButton.OK,
                MessageBoxImage.Question);
        }
        #endregion

        #region Подтверждения
        // Вызывает сообщение с подтверждением о выходе/закрытии окна
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

        // Подтверждение добавления
        public static bool ConfirmSave()
        {
            var resultChanged = MessageBox.Show("Вы уверены, что заполнили все поля верно?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resultChanged == MessageBoxResult.Yes)
            {
                MessageBox.Show("Добавление прошло успешно",
                    "Успех",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ConfirmEdit()
        {
            var resultChanged = MessageBox.Show("Вы уверены, что заполнили все поля верно?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resultChanged == MessageBoxResult.Yes)
            {
                MessageBox.Show("Изменение прошло успешно",
                    "Успех",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return true;
            }
            else
            {
                return false;
            }
        }

        // Подтверждение добавления ЗАЯВКИ
        public static bool ConfirmSaveApplication()
        {
            var resultChanged = MessageBox.Show("Вы уверены, что заполнили все поля верно? " +
                "После одобрения заявки внести изменения будет НЕВОЗМОЖНО!",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resultChanged == MessageBoxResult.Yes)
            {
                MessageBox.Show("Формирование заявки прошло успешно",
                    "Успех",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return true;
            }
            else
            {
                return false;
            }
        }

        // Подтверждение редактирования ЗАЯВКИ
        public static bool ConfirmEditApplication()
        {
            var resultChanged = MessageBox.Show("Вы уверены, что заполнили все поля верно? " +
                "После одобрения заявки внести изменения будет НЕВОЗМОЖНО!",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resultChanged == MessageBoxResult.Yes)
            {
                MessageBox.Show("Редактирование заявки прошло успешно",
                    "Успех",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return true;
            }
            else
            {
                return false;
            }
        }

        // Подтверждение отклонения ЗАЯВКИ
        public static bool ConfirmRejectApplication()
        {
            var resultChanged = MessageBox.Show("Вы уверены, что хотите отклонить заявку? " +
                "Данное действие отменить НЕВОЗМОЖНО!",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resultChanged == MessageBoxResult.Yes)
            {
                MessageBox.Show("Заявка октлонена успешно",
                    "Успех",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return true;
            }
            else
            {
                return false;
            }
        }

        // Подтверждение подтверждения ЗАЯВКИ
        public static bool ConfirmApplication()
        {
            var resultChanged = MessageBox.Show("Вы уверены, что хотите подтвердить заявку? " +
                "После данного действия заявка будет доступна для проведения инспекции ТС!",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resultChanged == MessageBoxResult.Yes)
            {
                MessageBox.Show("Заявка подтверждена успешно",
                    "Успех",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return true;
            }
            else
            {
                return false;
            }
        }

        // Подтверждение принятия ЗАЯВКИ на проведение инспекции 
        public static bool ConfirmAcceptForInspectionApplication()
        {
            var resultChanged = MessageBox.Show("Вы уверены, что хотите принять заявку? " +
                "После данного действия будут запланированы дата и время для проведения инспекции, " +
                "которую необходимо будет провести! В случае неявки гражданина, заявка и инспекция " +
                "будут автоматически анулированы",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resultChanged == MessageBoxResult.Yes)
            {
                MessageBox.Show("Заявка на проведение инспекции принята успешно",
                    "Успех",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
