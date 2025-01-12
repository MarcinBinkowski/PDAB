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

        public ObservableCollection<BaseWorkspaceViewModel> Workspaces
        {
            get => _workspaces;
            set
            {
                _workspaces = value;
                OnPropertyChanged(nameof(Workspaces));
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
            Workspaces.Add(viewModel);
        }

        private void ShowCustomers()
        {
            var viewModel = new AllCustomersViewModel(_repositoryFactory.GetRepository<Customer>());
            AddWorkspace(viewModel);
        }
        private void AddWorkspace(BaseWorkspaceViewModel workspace)
        {
            var existing = Workspaces.FirstOrDefault(w => w.DisplayName == workspace.DisplayName);
            if (existing != null)
            {
                Workspaces.Remove(existing);
            }
            Workspaces.Add(workspace);
        }
    }
}