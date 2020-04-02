using System.Windows;
using System.Windows.Controls;

namespace InvoicingSystem_SQLite.Components.TextBoxWIthHint
{
    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
    public class TextBoxWithHint : TextBox
    {
        private TextBox textBox;

        public string HintText
        {
            get => (string)GetValue(HintTextProperty);
            set => SetValue(HintTextProperty, value);
        }

        public static readonly DependencyProperty HintTextProperty =
            DependencyProperty.Register("HintText", typeof(string), typeof(TextBoxWithHint));

        public string ValidationError => Validation.GetErrors(textBox)[0].ErrorContent.ToString();

        static TextBoxWithHint()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxWithHint),
                new FrameworkPropertyMetadata(typeof(TextBoxWithHint)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            textBox = GetTemplateChild("PART_TextBox") as TextBox;
        }
    }
}
