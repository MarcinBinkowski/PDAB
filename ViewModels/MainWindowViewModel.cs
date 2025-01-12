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
        # endregion
        
        public MainWindowViewModel(IRepositoryFactory repositoryFactory)
        {
            DisplayName = "PDAB Marcin Binkowski";
            _repositoryFactory = repositoryFactory;
            Workspaces = new ObservableCollection<BaseWorkspaceViewModel>();
        }

        private void ShowCategories()
        {
            var categoryRepo = _repositoryFactory.GetRepository<Category>();
            var viewModel = new AllCategoriesViewModel(categoryRepo);
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