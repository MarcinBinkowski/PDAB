using PDAB.Models;

namespace PDAB.ViewModels
{
    public class AllCategoriesViewModel : BaseDataViewModel<Category>
    {
        public AllCategoriesViewModel(IRepository<Category> repository) 
            : base(repository, "Categories")
        {
        }
    }
}