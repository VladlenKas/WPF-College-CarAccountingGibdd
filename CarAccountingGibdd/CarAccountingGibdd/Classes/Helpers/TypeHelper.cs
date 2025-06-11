using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CarAccountingGibdd.Classes
{
    public static class TypeHelper
    {
        // Преобразовывает текст в дату либо возвращает мин. значение
        public static DateOnly DateOnlyParse(string str)
        {
            try
            {
                DateOnly number = DateOnly.Parse(str);
                return number;
            }
            catch
            {
                return DateOnly.MinValue;
            }
        }

        // Преобразовывает текст в строку либо возвращает 0
        public static decimal DecemalParse(string str)
        {
            try
            {
                decimal number = Decimal.Parse(str);
                return number;
            }
            catch
            {
                return 0;
            }
        }

        // Преобразовывает текст в строку либо возвращает 0
        public static int IntParse(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }
            else
            {
                int number = Convert.ToInt32(str);
                return number;
            }
        }

        // Преобразовывает текст в short либо возвращает -1
        public static short ShortParse(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return -1;
            }
            else
            {
                int year = Convert.ToInt32(str);
                return (short)year;
            }
        }

        public static string PaymentMethodToString(sbyte? method)
        {
            return method switch
            {
                0 => "Наличный расчет",
                1 => "Оплата картой",
                _ => "Заказ отменен"
            };
        }
    }
}
