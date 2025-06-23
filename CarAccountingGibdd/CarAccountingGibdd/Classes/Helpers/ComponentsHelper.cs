using CarAccountingGibdd.Components;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using CarAccountingGibdd;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace CarAccountingGibdd.Classes
{
    // Класс для работы с паролем
    public static class ComponentsHelper
    {
        /// <summary>
        /// Скрывает пароль
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="passHid"></param>
        /// <param name="passVis"></param>
        public static void ToggleVisibility(object sender, BindablePasswordBox passHid, TextBox passVis)
        {
            CheckBox checkbox = sender as CheckBox;
            if (checkbox.IsChecked == true)
            {
                // Vissible pass
                passVis.Text = passHid.Password;
                passVis.Visibility = Visibility.Visible;
                passHid.Visibility = Visibility.Hidden;
            }
            else
            {
                // Hidden pass
                passHid.Password = passVis.Text;
                passVis.Visibility = Visibility.Hidden;
                passHid.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Быстро получает пароль
        /// </summary>
        /// <param name="passHid"></param>
        /// <param name="passVis"></param>
        /// <returns></returns>
        public static string GetPassword(BindablePasswordBox passHid, TextBox passVis)
        {
            var pass = passVis.Visibility is Visibility.Visible ? passVis.Text : passHid.Password;
            return pass;
        }

        /// <summary>
        /// Затемняет область экрана при открытии информации
        /// </summary>
        /// <param name="infoWindow"></param>
        /// <param name="page"></param>
        public static void ShowDialogWindowDark(Window infoWindow)
        {
            App.MenuWindow.Opacity = 0.5;
            infoWindow.ShowDialog();
            App.MenuWindow.Opacity = 1;
        }
        /// <summary>
        /// Затемняет область экрана при открытии информации
        /// </summary>
        /// <param name="infoWindow"></param>
        /// <param name="page"></param>
        public static void ShowWindowDark(Window infoWindow)
        {
            App.MenuWindow.Opacity = 0.5;
            infoWindow.Show();
            App.MenuWindow.Opacity = 1;
        }

        private static readonly string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_-+=<>?";

        public static string GeneratePassword()
        {
            var rng = new RNGCryptoServiceProvider();
            var regex = new Regex(@"^[a-zA-Z0-9!@#\$%\^&\*\(\)_\-\+=<>\?]{5,10}$");
            while (true)
            {
                int length = RandomNumber(5, 11); // верхняя граница не включается
                char[] chars = new char[length];
                byte[] data = new byte[length];
                rng.GetBytes(data);
                for (int i = 0; i < length; i++)
                {
                    chars[i] = validChars[data[i] % validChars.Length];
                }
                string password = new string(chars);
                if (regex.IsMatch(password))
                    return password;
            }
        }

        private static int RandomNumber(int minValue, int maxValue)
        {
            // Генерирует случайное число в диапазоне [minValue, maxValue)
            byte[] intBytes = new byte[4];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(intBytes);
            }
            int value = Math.Abs(BitConverter.ToInt32(intBytes, 0));
            return minValue + (value % (maxValue - minValue));
        }
    }
}