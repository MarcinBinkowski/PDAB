using PDAB.Models;

namespace PDAB.ViewModels
{
    public class NewCategoryViewModel : SingleEntityViewModel<Category>
    {
        public NewCategoryViewModel()
            : base("Category")
        {
            item = new Category();
        }

        public int CategoryId
        {
            get => item.CategoryId;
            set
            {
                item.CategoryId = value;
                OnPropertyChanged(() => CategoryId);
            }
        }

        public string CategoryName
        {
            get => item.CategoryName;
            set
            {
                item.CategoryName = value;
                OnPropertyChanged(() => CategoryName);
            }
        }

        public override void Save()
        {
            dbContext.Categories.Add(item);
            dbContext.SaveChanges();
        }
    }
}