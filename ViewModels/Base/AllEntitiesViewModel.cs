using GalaSoft.MvvmLight.Messaging;
using PDAB.Helpers;
using PDAB.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;


namespace PDAB.ViewModels
{
    public abstract class AllEntitiesViewModel<T> : BaseWorkspaceViewModel where T : class
    {
        private string _currentSortProperty;
        private ListSortDirection _currentSortDirection;

        #region DB
        protected readonly PdabDbContext dbContext;
        #endregion

        #region Command


        private BaseCommand _refreshCommand;
        public ICommand RefreshCommand // References in mainwindow.xaml
        {
            get
            {
                if (_refreshCommand == null)
                    _refreshCommand = new BaseCommand(() => Load()); // Load is implemented on all ViewModels
                return _refreshCommand;
            }
        }
        private BaseCommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                Console.WriteLine("DeleteCommand");
                if (_deleteCommand == null)
                    _deleteCommand = new BaseCommand(
                        () => DeleteSelected(),
                        () => SelectedItem != null);
                return _deleteCommand;
            }
        }

        private BaseCommand<string> _sortCommand;
        public ICommand SortCommand
        {
            get
            {
                if (_sortCommand == null)
                    _sortCommand = new BaseCommand<string>(SortBy);
                return _sortCommand;
            }
        }
        
        #endregion

        #region List
        private ObservableCollection<T> _List;
        public ObservableCollection<T> List
        {
            get
            {
                if (_List == null)
                    Load();
                return _List;
            }
            set
            {
                _List = value;
                OnPropertyChanged(() => List);
            }
        }
        #endregion

        #region Constructor
        public AllEntitiesViewModel(String displayName)
        {
            dbContext = new PdabDbContext();
            base.DisplayName = displayName;
        }
        #endregion

        #region Helpers
        public abstract void Load();

        protected T _selectedItem; // set property on every AllView
        public T SelectedItem 
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                Console.WriteLine($"SelectedItem changed to: {value}");
                OnPropertyChanged(() => SelectedItem);
                (_deleteCommand as BaseCommand)?.RaiseCanExecuteChanged();
            }
        }
        public class CountResult
        {
            public int Count { get; set; }
        }

        private bool HasReferences(T entity)
        {
            // had problems with EF
            var entityType = dbContext.Model.FindEntityType(typeof(T));
            var referencingForeignKeys = entityType.GetReferencingForeignKeys();
            var primaryKeyProperty = entityType.FindPrimaryKey().Properties[0];
            var primaryKeyValue = primaryKeyProperty.PropertyInfo.GetValue(entity);

            var connection = dbContext.Database.GetDbConnection();
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();

            try
            {
                foreach (var fk in referencingForeignKeys)
                {
                    var tableName = fk.DeclaringEntityType.GetTableName();
                    var columnName = fk.Properties.First().GetColumnName();

                    using var command = connection.CreateCommand();
                    command.CommandText = $"SELECT COUNT(1) FROM {tableName} WHERE {columnName} = @id";
            
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "@id";
                    parameter.Value = primaryKeyValue;
                    command.Parameters.Add(parameter);

                    var count = (int)command.ExecuteScalar();
                    if (count > 0)
                    {
                        return true;
                    }
                }
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }

            return false;
        }
        
        private void DeleteSelected()
        {
            if (SelectedItem == null) return;

            if (HasReferences(SelectedItem))
            {
                MessageBox.Show(
                    "This record is referenced by other tables and cannot be deleted.\nRemove related records first.",
                    "Delete Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            try
            {
                dbContext.Set<T>().Remove(SelectedItem);
                dbContext.SaveChanges();
                Load();
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show("Error deleting record.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void SortBy(string propertyName)
        {
            var view = CollectionViewSource.GetDefaultView(List);
            view.SortDescriptions.Clear();

            if (propertyName == _currentSortProperty)
            {
                _currentSortDirection = _currentSortDirection == ListSortDirection.Ascending
                    ? ListSortDirection.Descending
                    : ListSortDirection.Ascending;
            }
            else
            {
                _currentSortProperty = propertyName;
                _currentSortDirection = ListSortDirection.Ascending;
            }

            view.SortDescriptions.Add(new SortDescription(_currentSortProperty, _currentSortDirection));
            OnPropertyChanged(() => List);
        }
        #endregion
    }
}