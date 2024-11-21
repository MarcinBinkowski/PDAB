using System;
using System.Windows.Input;

namespace PDAB.Helpers
{
    internal class BaseCommand : ICommand
    {
        private readonly Action _command;
        private readonly Func<bool> _canExecute;

        public BaseCommand(Action command, Func<bool> canExecute = null)
        {
            ArgumentNullException.ThrowIfNull(nameof(command));
            _command = command;
            _canExecute = canExecute;
        }

        public void Execute(object parameter)
        {
            Console.WriteLine($"BaseCommand.Execute called");
            _command();
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler CanExecuteChanged;
    }
}