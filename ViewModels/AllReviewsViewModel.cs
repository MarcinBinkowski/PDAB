using System.Collections.ObjectModel;
using PDAB.Models;

namespace PDAB.ViewModels
{
public class AllReviewsViewModel : BaseDataViewModel<Review>
{
    public AllReviewsViewModel(IRepository<Review> repository) 
        : base(repository, "Reviews")
    {
    }
}
}