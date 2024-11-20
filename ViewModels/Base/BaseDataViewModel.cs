using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using PDAB.Helpers;

namespace PDAB.ViewModels
{
    public class BaseDataViewModel<T> : BaseWorkspaceViewModel, IRefreshable where T : class
    {
        private readonly IRepository<T> _repository;
        private ObservableCollection<T> _items;
        public event EventHandler SelectionChanged;

        public ObservableCollection<T> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
                Console.WriteLine($"Selected collection changed to: {value}");
            }
        }
        
        private T _selectedItem;
        public T SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
                Console.WriteLine($"Selected item changed to: {value?.GetType().Name} - {value}");
                CommandManager.InvalidateRequerySuggested();
            }
        }
        
        public BaseDataViewModel(IRepository<T> repository, string displayName)
        {
            DisplayName = displayName;
            _repository = repository;
            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                Items = await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Error loading data: {ex.Message}");
            }
        }


        public async Task RefreshAsync()
        {
            try
            {
                Items = await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Error refreshing data: {ex.Message}", MessageBoxImage.Error);
            }
        }
          
        public async Task DeleteSelectedItem()
        {
            if (SelectedItem == null) return;
            Console.WriteLine("Deleting selected item: " + SelectedItem);

            if (MessageBox.Show($"Are you sure you want to delete this {typeof(T).Name}?", 
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    await _repository.DeleteAsync(SelectedItem);
                    await _repository.SaveChangesAsync();
                    await RefreshAsync();
                }
                catch (Exception ex)
                {
                    ShowMessageBox($"Error deleting {typeof(T).Name}: {ex.Message}", MessageBoxImage.Error);
                    if (ex.InnerException != null)
                    {
                        ShowMessageBox($"Inner exception: {ex.InnerException.Message}", MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}