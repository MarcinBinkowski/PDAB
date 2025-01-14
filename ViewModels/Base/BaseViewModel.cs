using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;
using PDAB.Helpers;


namespace PDAB.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region DisplayName
        public virtual string DisplayName { get; protected set; }
        #endregion
        #region WindowPropertys

        public void ShowMessageBox(string message, MessageBoxImage icon = MessageBoxImage.Error)
        {
            MessageBox.Show(message, "", MessageBoxButton.OK, icon);
        }
        
        public ICommand Close
        {
            get { return new BaseCommand(CloseApplication); }
        }

        public ICommand Maximize
        {
            get { return new BaseCommand(MaximizeApplication); }
        }

        public ICommand Minimize
        {
            get { return new BaseCommand(MinimizeApplication); }
        }

        public ICommand DragMove
        {
            get { return new BaseCommand(DragMoveCommand); }
        }

        public ICommand Restart
        {
            get { return new BaseCommand(RestartCommand); }
        }

        private static void RestartCommand()
        {
            Application.Current.Shutdown();
        }

        private static void DragMoveCommand()
        {
            Application.Current.MainWindow.DragMove();
        }

        private static void CloseApplication()
        {
            Application.Current.Shutdown();
        }

        private static void MaximizeApplication()
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            else
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
        }

        private static void MinimizeApplication()
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Minimized)
            {
                Application.Current.MainWindow.Opacity = 1;
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
            else
            {
                Application.Current.MainWindow.Opacity = 0;
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
            }
        }

        #endregion

        #region Propertychanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnPropertyChanged<T>(Expression<Func<T>> action)
        {
            var propertyName = GetPropertyName(action);
            OnPropertyChanged(propertyName);
        }

        private string GetPropertyName<T>(Expression<Func<T>> action)
        {
            if (action.Body is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }
        
            throw new ArgumentException("Expression must be a property access", nameof(action));
        }


        #endregion
    }
}