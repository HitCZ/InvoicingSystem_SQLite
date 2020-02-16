using InvoicingSystem_SQLite.Logic.Extensions;
using System.Windows.Controls;
using System.Windows.Input;

namespace InvoicingSystem_SQLite.Components
{
    class NumericTextBox : TextBox
    {
        public NumericTextBox()
        {
            PreviewTextInput += OnPreviewTextInput;
        }

        #region Event handlers

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!e.Text.IsNumber())
                e.Handled = true;
        }

        #endregion Event handlers
    }
}
