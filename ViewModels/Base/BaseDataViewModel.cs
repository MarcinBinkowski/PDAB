using System.Collections.ObjectModel;

public class BaseDataViewModel<T> : BaseWorkspaceViewModel where T : class
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
        Items = await _repository.GetAllAsync();
    }
}