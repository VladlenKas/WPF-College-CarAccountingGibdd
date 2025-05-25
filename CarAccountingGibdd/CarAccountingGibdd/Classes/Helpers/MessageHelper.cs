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
        
        // Предупреждение о пустых полях
        public static void MessageActiveApplication()
        {
            MessageBox.Show($"В настоящее время выбранное транспортное средство уже имеет" +
                $" действующую заявку. Гражданин не может подавать сразу несколько" +
                $" заявок на одно транспортное средство.",
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

        // Подтверждение добавления ЗАЯВКИ
        public static bool ConfirmSaveApplication()
        {
            var resultChanged = MessageBox.Show("Вы уверены, что заполнили все поля верно? " +
                "Внести изменения после принятия заявки на осмотр инспектором будет невозомжно.",
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
        #endregion
    }
}
