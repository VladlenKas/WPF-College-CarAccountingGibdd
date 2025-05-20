using CarAccoutingGibdd.Components;
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
using CarAccoutingGibdd;

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
    }
}