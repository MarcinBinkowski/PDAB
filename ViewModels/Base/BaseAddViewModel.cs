using System.Windows;
using System.Windows.Input;
using PDAB.Helpers;


namespace PDAB.ViewModels
{
    public abstract class BaseAddViewModel<T> : BaseWorkspaceViewModel where T : class, new()
    {
        protected readonly IRepository<T> _repository;
        private T _entity;
        protected ICommand _saveCommand;
        public ICommand SaveCommand => _saveCommand ??= new BaseCommand(async () => 
        {
            Console.WriteLine("Save Command Executed");
            await SaveAsync();
        });


        public T Entity
        {
            get => _entity;
            set
            {
                _entity = value;
                OnPropertyChanged(nameof(Entity));
            }
        }

        protected BaseAddViewModel(IRepository<T> repository, string displayName)
        {
            DisplayName = displayName;
            _repository = repository;
            Entity = new T();
        }

        protected virtual async Task SaveAsync()
        {
            Console.WriteLine("SaveAsync called");
            try
            {
                await _repository.AddAsync(Entity);
                await _repository.SaveChangesAsync();
                ShowMessageBox($"{typeof(T).Name} added successfully", MessageBoxImage.Information);
                OnRequestClose(); 
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Error saving {typeof(T).Name}: {ex.Message}");
                if (ex.InnerException != null)
                {
                    ShowMessageBox($"Inner Exception: {ex.InnerException.Message}");
                }
            }
        }
    }
}