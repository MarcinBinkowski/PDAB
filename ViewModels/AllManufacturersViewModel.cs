using System.Collections.ObjectModel;
using PDAB.Models;

public class AllManufacturersViewModel : BaseDataViewModel<Manufacturer>
{
    public AllManufacturersViewModel(IRepository<Manufacturer> repository) 
        : base(repository, "Manufacturers")
    {
    }
}