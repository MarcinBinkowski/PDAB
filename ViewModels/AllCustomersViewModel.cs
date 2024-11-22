using System.Collections.ObjectModel;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class AllCustomersViewModel : AllEntitiesViewModel<Customer>
    {
        public AllCustomersViewModel() : base("Customers")
        {
            Load();
        }

        public override void Load()
        {
            List = new ObservableCollection<Customer>(
                dbContext.Customers.ToList()
            );
        }
    }
}