
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class AddCategoryViewModel : BaseAddViewModel<Category>
    {
        public AddCategoryViewModel(IRepository<Category> repository) 
            : base(repository, "Add Category")
        {
        }
    }
}
