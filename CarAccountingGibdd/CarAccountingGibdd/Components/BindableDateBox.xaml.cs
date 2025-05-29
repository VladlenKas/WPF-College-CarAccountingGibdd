using CarAccountingGibdd.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CarAccountingGibdd.Components
{
    /// <summary>
    /// Логика взаимодействия для BindableDateBox.xaml
    /// </summary>

    /// <summary>
    /// Логика взаимодействия для BindableDateBox.xaml
    /// </summary>
    public partial class BindableDateBox : UserControl
    {
        private bool _isUpdatingText;

        // Зависимое свойство для пути к отображаемому свойству (DateTextProperty)
        public static readonly DependencyProperty DateTextProperty =
            DependencyProperty.Register("DateText", typeof(string), typeof(BindableDateBox),
                new FrameworkPropertyMetadata(string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnDateTextChanged));

        // Зависимое свойство для пути к отображаемому свойству (PlaceholderProperty)
        public static readonly DependencyProperty PlaceholderDateProperty =
            DependencyProperty.Register("PlaceholderDate", typeof(string), typeof(AutoCompleteTextBox), new PropertyMetadata(string.Empty));

        // Текст даты
        public string DateText
        {
            get => (string)GetValue(DateTextProperty);
            set => SetValue(DateTextProperty, value);
        }

        // Текст плейсхолдера
        public string PlaceholderDate
        {
            get { return (string)GetValue(PlaceholderDateProperty); }
            set { SetValue(PlaceholderDateProperty, value); }
        }

        public BindableDateBox()
        {
            InitializeComponent();
        }

        private static void OnDateTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BindableDateBox control && !control._isUpdatingText)
            {
                control.dateBox.Text = e.NewValue?.ToString();
            }
        }

        private void DateBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isUpdatingText) return;

            _isUpdatingText = true;

            var text = dateBox.Text.Replace(".", "");
            var newText = FormatDateText(text);

            DateText = newText;
            dateBox.Text = newText;
            dateBox.CaretIndex = newText.Length;

            _isUpdatingText = false;
        }

        private string FormatDateText(string input)
        {
            if (input.Length > 8) input = input.Substring(0, 8);

            return input.Length switch
            {
                > 4 => $"{input.Substring(0, 2)}.{input.Substring(2, 2)}.{input.Substring(4)}",
                > 2 => $"{input.Substring(0, 2)}.{input.Substring(2)}",
                _ => input
            };
        }

        private void DateBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                var text = dateBox.Text;
                if (text.EndsWith("."))
                {
                    e.Handled = true;
                    dateBox.Text = text.Substring(0, text.Length - 2);
                    dateBox.CaretIndex = dateBox.Text.Length;
                }
            }
        }

        private void DateBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Validations.ValidateInputNumbers(e);
        }

        private void DateBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            Validations.ValidatePasteNumbers(e);
        }
    }
}