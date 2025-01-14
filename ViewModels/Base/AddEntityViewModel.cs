using System.Windows.Input;
using PDAB.Helpers;

namespace PDAB.ViewModels;

public class AddEntityViewModel<T> : BaseWorkspaceViewModel where T : class, new()
{
    private readonly IRepository<T> _repository;
    private bool _hasChanges;
    private T _entity;
    public T Entity
    {
        get => _entity;
        set
        {
            _entity = value;
            HasChanges = true;
            OnPropertyChanged(nameof(Entity));
        }
    }
    
    public override bool HasChanges 
    { 
        get => _hasChanges;
        protected set
        {
            _hasChanges = value;
            OnPropertyChanged(nameof(HasChanges));
        }
    }
    
    public AddEntityViewModel(IRepository<T> repository, string displayName)
    {
        _repository = repository;
        Entity = new T();
        DisplayName = displayName;
    }


    public override async Task SaveAsync()
    {
        await _repository.AddAsync(Entity);
        await _repository.SaveChangesAsync();
        HasChanges = false;
}
}