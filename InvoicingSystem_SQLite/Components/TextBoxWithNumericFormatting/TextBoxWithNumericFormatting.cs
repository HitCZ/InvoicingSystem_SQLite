using InvoicingSystem_SQLite.Logic.Extensions;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace InvoicingSystem_SQLite.Components.TextBoxWithNumericFormatting
{
    class TextBoxWithNumericFormatting : TextBox
    {
        public TextBoxWithNumericFormatting()
        {
            TextChanged += OnTextChanged;
            PreviewTextInput += OnPreviewTextInput;
        }

        #region Event Handlers

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!e.Changes.Any(c => c.RemovedLength > 0))
                return;
            Text = Text.FormatIntoNumbers();
            CaretIndex = Text.Length;
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var separator = " ";

            if (e.Text != separator || !e.Text.IsNumber())
                e.Handled = true;

            if (e.Text == separator)
                return;

            var currentText = Text;
            var newText = currentText + e.Text;
            var formattedText = newText.FormatIntoNumbers();

            Text = formattedText;
            CaretIndex = Text.Length;
            e.Handled = true;
        }

        #endregion Event Handlers
    }
}
