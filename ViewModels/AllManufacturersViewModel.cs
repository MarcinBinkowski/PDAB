using System.Collections.ObjectModel;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class AllManufacturersViewModel : AllEntitiesViewModel<Manufacturer>
    {
        public AllManufacturersViewModel() : base("Manufacturers")
        {
            Load();
        }

        public override void Load()
        {
            List = new ObservableCollection<Manufacturer>(
                dbContext.Manufacturers.ToList()
            );
        }
    }
}