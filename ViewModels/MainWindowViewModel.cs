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
        private ObservableCollection<BaseWorkspaceViewModel?> _workspaces;
        private BaseWorkspaceViewModel _activeWorkspace;
        private BaseWorkspaceViewModel _previousWorkspace;
        private BaseCommand _refreshCommand;
        private BaseCommand _deleteCommand;

        public ICommand AddCommand => new BaseCommand(AddNew);
        public ICommand RefreshCommand => _refreshCommand ??= new BaseCommand(
            execute: async () => await RefreshActiveWorkspace(),
            canExecute: () => CanRefresh()
        );
        public ICommand DeleteCommand => _deleteCommand ??= new BaseCommand(
            execute: async () => await DeleteSelectedItem(),
            canExecute: () => GetSelectedItem() != null
        );

        private object? GetSelectedItem()
        {
            Console.WriteLine("GetSelectedItem called");
            return ActiveWorkspace switch
            {
                BaseDataViewModel<Category> categoryView => categoryView.SelectedItem,
                BaseDataViewModel<Customer> customerView => customerView.SelectedItem,
                _ => null
            };
        }

        public ObservableCollection<BaseWorkspaceViewModel?> Workspaces
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
                _refreshCommand?.RaiseCanExecuteChanged();
                _deleteCommand?.RaiseCanExecuteChanged();
                Console.WriteLine($"ActiveWorkspace changed to: {value?.GetType().Name}");

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
            Workspaces = new ObservableCollection<BaseWorkspaceViewModel?>();

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
        
        

        private void AddNew()
        {
            BaseWorkspaceViewModel? viewModel = ActiveWorkspace switch
            {
                BaseDataViewModel<Category> categoryView => 
                    new AddCategoryViewModel(_repositoryFactory.GetRepository<Category>()),
                BaseDataViewModel<Customer> customerView => 
                    new AddCustomerViewModel(_repositoryFactory.GetRepository<Customer>()),
                _ => null
            };

            if (viewModel != null)
            {
                _previousWorkspace = ActiveWorkspace;
                viewModel.RequestClose += async (s, e) => 
                {
                    Workspaces.Remove(viewModel);
                    if (_previousWorkspace is BaseDataViewModel<Category> categoryView)
                    {
                        await categoryView.RefreshAsync();
                        Workspaces.Add(categoryView);
                    }
                    else if (_previousWorkspace is BaseDataViewModel<Customer> customerView)
                    {
                        await customerView.RefreshAsync();
                        Workspaces.Add(customerView);
                    }
                };
                Workspaces.Clear();
                Workspaces.Add(viewModel);
            }
        }
        private bool CanRefresh()
        {
            Console.WriteLine("CanRefresh called");
            return ActiveWorkspace is IRefreshable;
        }
        private async Task RefreshActiveWorkspace()
        {
            Console.WriteLine("RefreshActiveWorkspace called");
            if (ActiveWorkspace is IRefreshable refreshable)
            {
                await refreshable.RefreshAsync();
            }
        }

        private async Task DeleteSelectedItem()
        {
            var item = GetSelectedItem();
            if (ActiveWorkspace is IDeletable deletable && item != null)
            {
                await deletable.DeleteItemAsync(item);
            }
        }
 
    }
}