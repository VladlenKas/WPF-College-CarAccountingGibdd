using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace CarAccountingGibdd.Classes
{
    public static partial class Validations 
    {
        #region Валидация на написание текста

        /// <summary>
        /// Проверяет, что вводимый текст состоит только из кириллических символов.
        /// </summary>
        public static void ValidateInputCyrillic(TextCompositionEventArgs e)
        {
            ValidateInput(e, Cyrillic());
        }

        /// <summary>
        /// Проверяет, что вводимый текст состоит только из цифр.
        /// </summary>
        public static void ValidateInputNumbers(TextCompositionEventArgs e)
        {
            ValidateInput(e, Numbers());
        }

        /// <summary>
        /// Проверяет, что вводимый текст соответствует формату описания.
        /// </summary>
        public static void ValidateInputDescription(TextCompositionEventArgs e)
        {
            ValidateInput(e, Description());
        }

        /// <summary>
        /// Проверяет, что вводимый текст соответствует формату электронной почты.
        /// </summary>
        public static void ValidateInputEmail(TextCompositionEventArgs e)
        {
            ValidateInput(e, Email());
        }

        /// <summary>
        /// Проверяет, что вводимый текст соответствует формату пароля.
        /// </summary>
        public static void ValidateInputPassword(TextCompositionEventArgs e)
        {
            ValidateInput(e, Password());
        }

        /// <summary>
        /// Проверяет, что вводимый текст соответствует формату типа веса.
        /// </summary>
        public static void ValidateInputWeight(TextCompositionEventArgs e)
        {
            ValidateInput(e, Weight());
        }

        /// <summary>
        /// Проверяет, что вводимый текст соответствует формату типа веса.
        /// </summary>
        public static void ValidateInputCyrillicAndNumbers(TextCompositionEventArgs e)
        {
            ValidateInput(e, CyrillicAndNumbers());
        }

        #endregion

        #region Валидация на вставку текста

        /// <summary>
        /// Проверяет вставляемый текст на соответствие кириллическим символам.
        /// </summary>
        public static void ValidatePasteCyrillic(DataObjectPastingEventArgs e)
        {
            ValidatePaste(e, Cyrillic());
        }

        /// <summary>
        /// Проверяет вставляемый текст на соответствие числам.
        /// </summary>
        public static void ValidatePasteNumbers(DataObjectPastingEventArgs e)
        {
            ValidatePaste(e, Numbers());
        }

        /// <summary>
        /// Проверяет вставляемый текст на соответствие формату описания для адреса.
        /// </summary>
        public static void ValidatePasteDescription(DataObjectPastingEventArgs e)
        {
            ValidatePaste(e, Description());
        }

        /// <summary>
        /// Проверяет вставляемый текст на соответствие формату электронной почты.
        /// </summary>
        public static void ValidatePasteEmail(DataObjectPastingEventArgs e)
        {
            ValidatePaste(e, Email());
        }

        /// <summary>
        /// Проверяет вставляемый текст на соответствие формату электронной почты.
        /// </summary>
        public static void ValidatePasteWeight(DataObjectPastingEventArgs e)
        {
            ValidatePaste(e, Weight());
        }
        
        /// <summary>
        /// Проверяет вставляемый текст на соответствие формату электронной почты.
        /// </summary>
        public static void ValidatePasteCyrillicAndNumbers(DataObjectPastingEventArgs e)
        {
            ValidatePaste(e, CyrillicAndNumbers());
        }

        /// <summary>
        /// Проверяет вставляемый текст на соответствие формату пароля.
        /// </summary>
        public static void ValidatePastePassword(DataObjectPastingEventArgs e)
        {
            ValidatePaste(e, Password());
        }

        #endregion

        /// <summary>
        /// Извлекает текст из буфера обмена при вставке.
        /// </summary>
        private static string GetTextFromPaste(DataObjectPastingEventArgs e)
        {
            return e.DataObject.GetData(DataFormats.Text)?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Проверяет, соответствует ли вводимый текст заданному регулярному выражению.
        /// </summary>
        private static void ValidateInput(TextCompositionEventArgs e, Regex regex)
        {
            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true; // Блокируем ввод
            }
        }

        /// <summary>
        /// Проверяет, соответствует ли вставляемый текст заданному регулярному выражению.
        /// </summary>
        private static void ValidatePaste(DataObjectPastingEventArgs e, Regex regex)
        {
            string pastedText = GetTextFromPaste(e);
            if (!regex.IsMatch(pastedText))
            {
                e.CancelCommand(); // Блокируем вставку
            }
        }

        // Кирилика
        [GeneratedRegex(@"[а-яА-Я]")]
        private static partial Regex Cyrillic();

        // Цифры
        [GeneratedRegex(@"[0-9]")]
        private static partial Regex Numbers();

        // Буквы и цифры
        [GeneratedRegex(@"[а-яА-Я0-9]")]
        private static partial Regex CyrillicAndNumbers();

        // Описание
        [GeneratedRegex(@"[а-яА-Я0-9-().,;""':/]")]
        private static partial Regex Description();

        // Эл. почта
        [GeneratedRegex(@"[a-zA-Z0-9\@\.]")]
        private static partial Regex Email();

        // Цифры для веса содержмиого заказа
        [GeneratedRegex(@"[0-9\,]")]
        private static partial Regex Weight();

        // Пароль
        [GeneratedRegex(@"[a-zA-Z!@#$&*0-9]")]
        private static partial Regex Password();

        // Ограничение по возрасту 
        public static bool ValidateCorrectAge(DateOnly date)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            int age = today.Year - date.Year;

            if (today.Month < date.Month || (today.Month == date.Month && today.Day < date.Day))
            {
                age--;
            }

            if (age < 18 || age > 80)
            {
                return false;
            }
            return true;
        }

        // Ограничение на валидность почты
        public static bool ValidateCorrectEmail(string email)
        {
            var regex = new Regex(@"^[a-zA-Z0-9]+@[a-zA-Z0-9]+\.[a-zA-Z]{2,4}$");
            if (!regex.IsMatch(email))
            {
                return false;
            }
            return true;
        }

        // Ограничение на валидность лицензионного номера
        public static bool ValidateCorrectLicensePlate(string licensePlate)
        {
            var regex = new Regex(@"^[А-Я]{1}[0-9]{3}[А-Я]{2}$");
            if (!regex.IsMatch(licensePlate))
            {
                return false;
            }
            return true;
        }
    }
}
