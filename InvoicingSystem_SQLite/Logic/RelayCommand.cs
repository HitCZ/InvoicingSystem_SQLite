using System;
using System.Windows.Input;

namespace InvoicingSystem_SQLite.Logic
{
    public class RelayCommand : ICommand
    {
        private readonly Action executeAction;
        private readonly Func<bool> canExecuteAction;

        public RelayCommand(Action executeAction) : this(executeAction, null)
        {
        }

        public RelayCommand(Action executeAction, Func<bool> canExecuteAction)
        {
            this.executeAction = executeAction ?? throw new ArgumentNullException(nameof(executeAction));
            this.canExecuteAction = canExecuteAction;
        }

        public bool CanExecute()
        {
            if (canExecuteAction is null)
                return true;
            var canExecute = canExecuteAction();

            return canExecute;
        }

        public void Execute() => executeAction();

        public bool CanExecute(object parameter) => CanExecute();

        public void Execute(object parameter) => Execute();

        public event EventHandler CanExecuteChanged;
    }
}
