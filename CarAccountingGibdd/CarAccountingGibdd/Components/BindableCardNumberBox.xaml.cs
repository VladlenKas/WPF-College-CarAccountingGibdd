using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CarAccountingGibdd.Components
{
    public partial class BindableCardNumberBox : UserControl
    {
        private bool _isUpdatingText;

        public static readonly DependencyProperty CardNumberProperty =
            DependencyProperty.Register("CardNumber", typeof(string), typeof(BindableCardNumberBox),
                new FrameworkPropertyMetadata(string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnCardNumberChanged));

        public static readonly DependencyProperty CardNumberPlaceholderProperty =
            DependencyProperty.Register("CardNumberPlaceholder", typeof(string), typeof(BindableCardNumberBox), new PropertyMetadata(string.Empty));

        public string CardNumber
        {
            get
            {
                var cleanNumber = (string)GetValue(CardNumberProperty);
                return string.IsNullOrEmpty(cleanNumber)
                    ? string.Empty
                    : cleanNumber.Replace(" ", "");
            }
            set => SetValue(CardNumberProperty, value);
        }

        public string CardNumberPlaceholder
        {
            get { return (string)GetValue(CardNumberPlaceholderProperty); }
            set { SetValue(CardNumberPlaceholderProperty, value); }
        }

        public BindableCardNumberBox()
        {
            InitializeComponent();
        }

        private static void OnCardNumberChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BindableCardNumberBox control && !control._isUpdatingText)
            {
                control.cardNumberBox.Text = control.FormatCardNumber(e.NewValue?.ToString() ?? string.Empty);
            }
        }

        private void CardNumberBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isUpdatingText) return;
            _isUpdatingText = true;

            // Оставляем только цифры
            var text = new string(cardNumberBox.Text.Where(char.IsDigit).ToArray());

            // Ограничиваем длину 16 цифрами
            if (text.Length > 16)
                text = text.Substring(0, 16);

            var formatted = FormatCardNumber(text);

            CardNumber = text; // CardNumber всегда только цифры
            cardNumberBox.Text = formatted;
            cardNumberBox.CaretIndex = formatted.Length;

            _isUpdatingText = false;
        }

        private string FormatCardNumber(string number)
        {
            // Формат: "1234 5678 9012 3456"
            if (string.IsNullOrEmpty(number))
                return string.Empty;

            number = new string(number.Where(char.IsDigit).ToArray());
            var parts = Enumerable.Range(0, number.Length / 4 + (number.Length % 4 == 0 ? 0 : 1))
                                  .Select(i => number.Substring(i * 4, Math.Min(4, number.Length - i * 4)));
            return string.Join(" ", parts);
        }

        private void CardNumberBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var pos = cardNumberBox.SelectionStart;

            // Удаление пробелов при Backspace/Delete
            if ((e.Key == Key.Back && pos > 0 && cardNumberBox.Text[pos - 1] == ' ') ||
                (e.Key == Key.Delete && pos < cardNumberBox.Text.Length && cardNumberBox.Text[pos] == ' '))
            {
                cardNumberBox.Text = cardNumberBox.Text.Remove(pos - (e.Key == Key.Back ? 1 : 0), 1);
                cardNumberBox.CaretIndex = pos - (e.Key == Key.Back ? 1 : 0);
                e.Handled = true;
            }
        }

        private void CardNumberBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // Если поле пустое или состоит только из пробелов
            if (string.IsNullOrWhiteSpace(cardNumberBox.Text.Replace(" ", "")))
            {
                _isUpdatingText = true;
                CardNumber = string.Empty;
                cardNumberBox.Text = string.Empty;
                _isUpdatingText = false;
            }
        }
    }
}
