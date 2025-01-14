using System.Collections.ObjectModel;
using System.Windows;

namespace PDAB.ViewModels
{
    public class BaseDataViewModel<T> : BaseWorkspaceViewModel, IRefreshable where T : class
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
        
    }
}