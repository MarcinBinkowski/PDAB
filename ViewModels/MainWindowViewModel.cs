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

        public ICommand AddCommand => new BaseCommand(AddNew);

        private void AddNew()
        {
            BaseWorkspaceViewModel viewModel = ActiveWorkspace switch
            {
                BaseDataViewModel<Category> => new AddCategoryViewModel(_repositoryFactory.GetRepository<Category>()),
                BaseDataViewModel<Customer> => new AddCustomerViewModel(_repositoryFactory.GetRepository<Customer>()),
                _ => null
            };

            if (viewModel != null)
            {
                viewModel.RequestClose += async (s, e) => 
                {
                    Workspaces.Remove(viewModel);
                    if (ActiveWorkspace is BaseDataViewModel dataView)
                    {
                        await dataView.RefreshAsync();
                    }
                };
                Workspaces.Clear();
                Workspaces.Add(viewModel);
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