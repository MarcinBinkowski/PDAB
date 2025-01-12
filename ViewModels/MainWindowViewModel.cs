using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using PDAB.Helpers;
using PDAB.Models;
using PDAB.ViewModels;

namespace PDAB.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly ObservableCollection<BaseWorkspaceViewModel> _workspaces;

        public MainWindowViewModel(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            _workspaces = new ObservableCollection<BaseWorkspaceViewModel>();
        }

        private void ShowCategories()
        {
            var categoryRepo = _repositoryFactory.GetRepository<Category>();
            var categoriesVm = new AllCategoriesViewModel(categoryRepo);
            _workspaces.Add(categoriesVm);
        }
    }
}