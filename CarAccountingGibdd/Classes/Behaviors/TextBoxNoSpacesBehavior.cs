using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CarAccountingGibdd.Classes.Behaviors
{
    public static class NoSpacesTextBoxBehavior
    {
        public static readonly DependencyProperty DisallowSpacesProperty =
            DependencyProperty.RegisterAttached(
                "DisallowSpaces",
                typeof(bool),
                typeof(NoSpacesTextBoxBehavior),
                new PropertyMetadata(false, OnDisallowSpacesChanged));

        public static bool GetDisallowSpaces(DependencyObject obj)
            => (bool)obj.GetValue(DisallowSpacesProperty);

        public static void SetDisallowSpaces(DependencyObject obj, bool value)
            => obj.SetValue(DisallowSpacesProperty, value);

        private static void OnDisallowSpacesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not TextBox textBox)
                return;

            bool isEnabled = (bool)e.NewValue;

            if (isEnabled)
            {
                textBox.PreviewTextInput += TextBox_PreviewTextInput;
                textBox.PreviewKeyDown += TextBox_PreviewKeyDown;      // Добавляем обработчик
                DataObject.AddPastingHandler(textBox, TextBox_Pasting);
            }
            else
            {
                textBox.PreviewTextInput -= TextBox_PreviewTextInput;
                textBox.PreviewKeyDown -= TextBox_PreviewKeyDown;      // Убираем обработчик
                DataObject.RemovePastingHandler(textBox, TextBox_Pasting);
            }
        }

        private static void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true; // Блокируем ввод пробела
            }
        }


        private static void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Contains(" "))
                e.Handled = true;
        }

        private static void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                string text = (string)e.DataObject.GetData(DataFormats.Text);
                if (text.Contains(" "))
                    e.CancelCommand();
            }
        }
    }
}
