using System.Collections.ObjectModel;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class AllCategoriesViewModel : AllEntitiesViewModel<Category>
    {
        private ObservableCollection<Category> _categories;
        private readonly IRepository<Category> _repository;

        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        public AllCategoriesViewModel(IRepository<Category> repository) : base("Categories")
        {
            _repository = repository;
            Load();
        }

        public override async void Load()
        {
            Categories = await _repository.GetAllAsync();
        }
    }
}