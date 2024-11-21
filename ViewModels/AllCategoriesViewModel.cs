using System.Collections.ObjectModel;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class AllCategoriesViewModel : AllEntitiesViewModel<Category>
    {
        public AllCategoriesViewModel()
            :base("Categories")
        {
            Load();
        }

        public override void Load()
        {
            List = new ObservableCollection<Category>(
                dbContext.Categories.ToList()
            );
        }
    }
}

