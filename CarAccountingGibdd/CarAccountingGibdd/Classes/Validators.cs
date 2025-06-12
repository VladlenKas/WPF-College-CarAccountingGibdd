using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using CarAccountingGibdd.Model;

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

        /// <summary>
        /// Проверяет, что вводимый текст соответствует формату типа веса.
        /// </summary>
        public static void ValidateInputVin(TextCompositionEventArgs e)
        {
            ValidateInput(e, Vin());
        }

        /// <summary>
        /// Проверяет, что вводимый текст соответствует формату номерного знака.
        /// </summary>
        public static void ValidateInputLicensePlate(TextCompositionEventArgs e)
        {
            ValidateInput(e, LicensePlate());
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

        /// <summary>
        /// Проверяет вставляемый текст на соответствие формату пароля.
        /// </summary>
        public static void ValidatePasteVin(DataObjectPastingEventArgs e)
        {
            ValidatePaste(e, Vin());
        }
        /// <summary>
        /// Проверяет вставляемый текст на соответствие формату пароля.
        /// </summary>
        public static void ValidatePasteLicensePlate(DataObjectPastingEventArgs e)
        {
            ValidatePaste(e, LicensePlate());
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

        // Для ввода ВИН
        [GeneratedRegex(@"[A-Z0-9]")]
        private static partial Regex Vin();

        // Номерной знак
        [GeneratedRegex(@"[А-Я0-9]")]
        private static partial Regex LicensePlate();

        // Описание
        [GeneratedRegex(@"[а-яА-Я0-9-().,;""':/]")]
        private static partial Regex Description();

        // Эл. почта
        [GeneratedRegex(@"[a-zA-Z0-9\@\.]")]
        private static partial Regex Email();

        // Цифры для веса 
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
        
        // Ограничение на валидность ВИН
        public static bool ValidateCorrectVin(string vin)
        {
            var regex = new Regex(@"^[A-HJ-NPR-Z0-9]{17}$");
            if (!regex.IsMatch(vin))
            {
                return false;
            }
            return true;
        }

        // Проверка на пустые строки
        public static bool StringEqualsNullOrEmpty(string? a, string? b)
        {
            return (a ?? "") == (b ?? "");
        }

        // Проверка на валидность номера карты
        public static bool ValidateCardNumber(Card card)
        {
            string? cardNumber = card.Number;

            // Проверка на пустой номер карты
            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                MessageHelper.MessageUniversal("Введите номер карты!");
                return false;
            }

            // Проверка на длину
            if (cardNumber.Length != 16)
            {
                MessageHelper.MessageUniversal("Номер карты должен содержать 16 цифр!");
                return false;
            }

            // Проверка на формат (первая цифра)
            if (!"23456".Contains(cardNumber[0]))
            {
                MessageHelper.MessageUniversal("Номер карты должен начинаться с 2, 3, 4, 5 или 6!");
                return false;
            }

            // Проверка на корректную дату
            if (!ValidateCardDate(card.Month, card.Year))
            {
                return false;
            }

            // Проверка на ввод кода
            if (card.Code == 0 || card.Code.ToString().Length != 3)
            {
                MessageHelper.MessageUniversal("Введите CVV/CVC код от карты в формате «NNN»!");
                return false;
            }

            return true;
        }

        // Проверку на ввод даты для данных карты
        private static bool ValidateCardDate(int? cardMonth, int? cardYear)
        {
            if (cardMonth == null || cardYear == null)
            {
                MessageHelper.MessageUniversal("Введите месяц и год окончания срока действия карты в формате «MM/yy»!");
                return false;
            }

            int month = cardMonth.Value;
            int year = cardYear.Value;

            if (month < 1 || month > 12)
            {
                MessageHelper.MessageUniversal("Месяц должен быть в диапазоне от 01 до 12!");
                return false;
            }

            // Проверяем, что срок действия не истек
            var now = DateTime.Now;
            var cardDate = new DateTime(year, month, 1).AddMonths(1).AddDays(-1); // последний день месяца

            if (cardDate < now.Date)
            {
                MessageHelper.MessageUniversal("Срок действия карты истёк!");
                return false;
            }

            return true;
        }
    }
}
