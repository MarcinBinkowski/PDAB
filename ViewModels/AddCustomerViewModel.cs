
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class AddCustomerViewModel : BaseAddViewModel<Customer>
    {
        public AddCustomerViewModel(IRepository<Customer> repository) 
            : base(repository, "Add Customer")
        {
        }
    }
}