using Microsoft.EntityFrameworkCore;
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

        public override bool Save()
        {
            try
            {
                dbContext.Categories.Add(item);
                dbContext.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }
    }
}