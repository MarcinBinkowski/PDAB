using System.Collections.ObjectModel;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class AllCategoriesViewModel : BaseWorkspaceViewModel
    {
        private readonly IRepository<Category> _categoryRepository;
        private ObservableCollection<Category> _categories;

        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        public AllCategoriesViewModel(IRepository<Category> categoryRepository)
        {
            DisplayName = "Categories";
            _categoryRepository = categoryRepository;
            LoadCategories();
        }

        private async void LoadCategories()
        {
            Categories = await _categoryRepository.GetAllAsync();
        }
    }
}