using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CarAccountingGibdd.Components
{
    public partial class BindableCardDateBox : UserControl
    {
        private bool _isUpdatingText;

        public static readonly DependencyProperty CardDateProperty =
            DependencyProperty.Register("CardDate", typeof(string), typeof(BindableCardDateBox),
                new FrameworkPropertyMetadata(string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnCardDateChanged));

        public static readonly DependencyProperty CardDatePlaceholderProperty =
            DependencyProperty.Register("CardDatePlaceholder", typeof(string), typeof(BindableCardDateBox), new PropertyMetadata(string.Empty));

        public string CardDate
        {
            get => (string)GetValue(CardDateProperty) ?? string.Empty;
            set => SetValue(CardDateProperty, value);
        }

        public int? CardMonth
        {
            get
            {
                if (string.IsNullOrEmpty(CardDate) || CardDate.Length != 4)
                    return null;

                return int.Parse(CardDate.Substring(0, 2));
            }
        }

        public int? CardYear
        {
            get
            {
                if (string.IsNullOrEmpty(CardDate) || CardDate.Length != 4)
                    return null;

                return 2000 + int.Parse(CardDate.Substring(2, 2));
            }
        }

        public string CardDatePlaceholder
        {
            get { return (string)GetValue(CardDatePlaceholderProperty); }
            set { SetValue(CardDatePlaceholderProperty, value); }
        }

        public BindableCardDateBox()
        {
            InitializeComponent();
        }

        private static void OnCardDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BindableCardDateBox control && !control._isUpdatingText)
            {
                control.cardDateBox.Text = control.FormatCardDate(e.NewValue?.ToString() ?? string.Empty);
            }
        }

        private void CardDateBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isUpdatingText) return;
            _isUpdatingText = true;

            // Оставляем только цифры
            var text = new string(cardDateBox.Text.Where(char.IsDigit).ToArray());

            // Ограничиваем длину 4 символами (MMYY)
            if (text.Length > 4)
                text = text.Substring(0, 4);

            var formatted = FormatCardDate(text);

            CardDate = text; // CardDate всегда только цифры
            cardDateBox.Text = formatted;
            cardDateBox.CaretIndex = formatted.Length;

            _isUpdatingText = false;
        }

        private string FormatCardDate(string number)
        {
            if (string.IsNullOrEmpty(number))
                return string.Empty;

            if (number.Length <= 2)
                return number;
            else
                return number.Insert(2, "/");
        }

        private void CardDateBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var pos = cardDateBox.SelectionStart;

            // Удаление слеша при Backspace/Delete
            if ((e.Key == Key.Back && pos > 0 && cardDateBox.Text[pos - 1] == '/') ||
                (e.Key == Key.Delete && pos < cardDateBox.Text.Length && cardDateBox.Text[pos] == '/'))
            {
                cardDateBox.Text = cardDateBox.Text.Remove(pos - (e.Key == Key.Back ? 1 : 0), 1);
                cardDateBox.CaretIndex = pos - (e.Key == Key.Back ? 1 : 0);
                e.Handled = true;
            }
        }

        private void CardDateBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // Если поле пустое или состоит только из пробелов/слешей
            if (string.IsNullOrWhiteSpace(cardDateBox.Text.Replace("/", "")))
            {
                _isUpdatingText = true;
                CardDate = string.Empty;
                cardDateBox.Text = string.Empty;
                _isUpdatingText = false;
            }
        }
    }
}
