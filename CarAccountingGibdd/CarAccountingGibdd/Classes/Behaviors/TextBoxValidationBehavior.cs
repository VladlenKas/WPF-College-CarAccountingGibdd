using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;

namespace CarAccountingGibdd.Classes
{
    public static class TextBoxValidationBehavior
    {
        public static readonly DependencyProperty ValidationTypeProperty =
            DependencyProperty.RegisterAttached(
                "ValidationType",
                typeof(ValidationType),
                typeof(TextBoxValidationBehavior),
                new PropertyMetadata(ValidationType.None, OnValidationTypeChanged));

        public static ValidationType GetValidationType(DependencyObject obj)
            => (ValidationType)obj.GetValue(ValidationTypeProperty);

        public static void SetValidationType(DependencyObject obj, ValidationType value)
            => obj.SetValue(ValidationTypeProperty, value);

        private static void OnValidationTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not TextBox textBox) return;

            textBox.PreviewTextInput -= TextBox_PreviewTextInput;
            DataObject.RemovePastingHandler(textBox, TextBox_Pasting);

            if ((ValidationType)e.NewValue != ValidationType.None)
            {
                textBox.PreviewTextInput += TextBox_PreviewTextInput;
                DataObject.AddPastingHandler(textBox, TextBox_Pasting);
            }
        }

        private static void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = (TextBox)sender;
            var type = GetValidationType(textBox);

            switch (type)
            {
                case ValidationType.Cyrillic:
                    Validations.ValidateInputCyrillic(e);
                    break;
                case ValidationType.Numbers:
                    Validations.ValidateInputNumbers(e);
                    break;
                case ValidationType.Description:
                    Validations.ValidateInputDescription(e);
                    break;
                case ValidationType.Email:
                    Validations.ValidateInputEmail(e);
                    break;
                case ValidationType.Password:
                    Validations.ValidateInputPassword(e);
                    break;
                case ValidationType.Weight:
                    Validations.ValidateInputWeight(e);
                    break;
                case ValidationType.CyrillicAndNumbers:
                    Validations.ValidateInputCyrillicAndNumbers(e);
                    break;
            }
        }

        private static void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            var textBox = (TextBox)sender;
            var type = GetValidationType(textBox);

            switch (type)
            {
                case ValidationType.Cyrillic:
                    Validations.ValidatePasteCyrillic(e);
                    break;
                case ValidationType.Numbers:
                    Validations.ValidatePasteNumbers(e);
                    break;
                case ValidationType.Description:
                    Validations.ValidatePasteDescription(e);
                    break;
                case ValidationType.Email:
                    Validations.ValidatePasteEmail(e);
                    break;
                case ValidationType.Password:
                    Validations.ValidatePastePassword(e);
                    break;
                case ValidationType.Weight:
                    Validations.ValidatePasteWeight(e);
                    break;
                case ValidationType.CyrillicAndNumbers:
                    Validations.ValidatePasteCyrillicAndNumbers(e);
                    break;
            }
        }
    }

    public enum ValidationType
    {
        None,
        Cyrillic,
        Numbers,
        Email,
        Password,
        Description,
        Weight,
        CyrillicAndNumbers
    }
}
