using System.Collections.ObjectModel;

public class BaseDataViewModel<T> : BaseWorkspaceViewModel where T : class
{
    private readonly IRepository<T> _repository;
    private ObservableCollection<T> _items;
    private bool _hasChanges;

    
    public override bool HasChanges 
    { 
        get => _hasChanges;
        protected set
        {
            _hasChanges = value;
            OnPropertyChanged(nameof(HasChanges));
        }
    }

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
    
    public override async Task SaveAsync()
    {
        await _repository.SaveChangesAsync();
        HasChanges = false;
    }
}