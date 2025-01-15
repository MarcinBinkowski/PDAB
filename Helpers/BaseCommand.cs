using System;
using System.Windows.Input;

namespace PDAB.Helpers
{
    public class BaseCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<bool> _canExecute;
        private bool _isExecuting;

        public BaseCommand(Action<T> execute, Func<bool> canExecute = null)
        {
            Console.WriteLine($"Creating command: {execute}, {canExecute}.");

            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => 
            !_isExecuting && (_canExecute?.Invoke() ?? true);

        public void Execute(object parameter)
        {
            if (_isExecuting) return;
        
            try
            {
                _isExecuting = true;
                RaiseCanExecuteChanged();
                _execute((T)parameter);
            }
            finally
            {
                ResetState();
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        private void ResetState()
        {
            _isExecuting = false;
            RaiseCanExecuteChanged();
        }
        

    }

    public class BaseCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public BaseCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;

        public void Execute(object parameter)
        {
            _execute();
        }

        public void RaiseCanExecuteChanged()
        {
            Console.WriteLine("RaiseCanExecuteChanged");
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}