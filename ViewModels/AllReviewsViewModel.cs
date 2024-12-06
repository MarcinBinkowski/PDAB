using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class AllReviewsViewModel : AllEntitiesViewModel<Review>
    {
        public AllReviewsViewModel() : base("Reviews")
        {
            Load();
        }

        public override void Load()
        {
            List = new ObservableCollection<Review>(
                dbContext.Reviews
                    .Include(r => r.Product)
                    .ToList()
            );
        }
    }
}