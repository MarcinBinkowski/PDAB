using System.Collections.ObjectModel;
using System.Windows;

namespace PDAB.ViewModels
{
    public class BaseDataViewModel<T> : BaseWorkspaceViewModel, IRefreshable, IDeletable where T : class
    {
        private readonly IRepository<T> _repository;
        private ObservableCollection<T> _items;
        public ObservableCollection<T> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
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
        public async Task DeleteItemAsync(object item)
        {
            if (item is T entityItem)
            {
                if (MessageBox.Show($"Are you sure you want to delete this {typeof(T).Name}?", 
                        "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _repository.DeleteAsync(entityItem);
                        await _repository.SaveChangesAsync();
                        await RefreshAsync();
                    }
                    catch (Exception ex)
                    {
                        ShowMessageBox($"Error deleting {typeof(T).Name}: {ex.Message}", MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}