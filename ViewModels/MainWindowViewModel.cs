using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using PDAB.Helpers;
using PDAB.Models;
using PDAB.Repository;

namespace PDAB.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private ObservableCollection<BaseWorkspaceViewModel> _workspaces;
        private BaseWorkspaceViewModel _activeWorkspace;
        private Type _currentEntityType;

        public ICommand SaveCommand => new BaseCommand(
            execute: async () => await SaveChanges(),
            canExecute: () => ActiveWorkspace?.HasChanges ?? false
        );
        private bool CanAdd() => _currentEntityType != null;

        private void AddNew()
        {
            switch (ActiveWorkspace)
            {
                case AllCategoriesViewModel:
                    ShowAddForm<Category>("Add Category");
                    break;
                case AllCustomersViewModel:
                    ShowAddForm<Customer>("Add Customer");
                    break;
            }
        }
        private async Task SaveChanges()
        {
            if (ActiveWorkspace is ISaveable saveable)
            {
                await saveable.SaveAsync();
            }
        }
        
        public ObservableCollection<BaseWorkspaceViewModel> Workspaces
        {
            get => _workspaces;
            set
            {
                _workspaces = value;
                OnPropertyChanged(nameof(Workspaces));
            }
        }
        public BaseWorkspaceViewModel ActiveWorkspace
        {
            get => _activeWorkspace;
            set
            {
                _activeWorkspace = value;
                OnPropertyChanged(nameof(ActiveWorkspace));
            }
        }

        
        # region commands
        public ICommand ShowCategoriesCommand => new BaseCommand(() => ShowCategories());
        public ICommand ShowCustomersCommand => new BaseCommand(() => ShowCustomers());

        # endregion
        
        public MainWindowViewModel(IRepositoryFactory repositoryFactory)
        {
            DisplayName = "PDAB Marcin Binkowski";
            _repositoryFactory = repositoryFactory;
            Workspaces = new ObservableCollection<BaseWorkspaceViewModel>();
        }
        private void ShowAddForm<T>(string displayName) where T : class, new()
        {
            var addViewModel = new AddEntityViewModel<T>(_repositoryFactory.GetRepository<T>(), displayName);
            Workspaces.Clear();
            Workspaces.Add(addViewModel);
        }
        
        private void ShowCategories()
        {
            Console.WriteLine("ShowCategories called");
            var viewModel = new AllCategoriesViewModel(_repositoryFactory.GetRepository<Category>());
            AddWorkspace(viewModel);
        }

        private void ShowCustomers()
        {
            var viewModel = new AllCustomersViewModel(_repositoryFactory.GetRepository<Customer>());
            AddWorkspace(viewModel);
        }
        private void AddWorkspace(BaseWorkspaceViewModel workspace)
        {
            Workspaces.Clear(); 
            Workspaces.Add(workspace);
            ActiveWorkspace = workspace;
        }
    }
}