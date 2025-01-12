using System.Collections.ObjectModel;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class AllCustomersViewModel : BaseDataViewModel<Customer>
    {
        public AllCustomersViewModel(IRepository<Customer> repository) 
            : base(repository, "Customers")
        {
        }
    }
}