
using PDAB.Models;
using PDAB.ViewModels;

public class AllManufacturersViewModel : BaseDataViewModel<Manufacturer>
{
    public AllManufacturersViewModel(IRepository<Manufacturer> repository) 
        : base(repository, "Manufacturers")
    {
    }
}