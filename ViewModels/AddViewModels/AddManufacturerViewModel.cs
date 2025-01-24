using System.Windows.Input;
using PDAB.Helpers;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class AddManufacturerViewModel : BaseAddViewModel<Manufacturer>
    {
        public AddManufacturerViewModel(IRepository<Manufacturer> repository)
            : base(repository, "Add Manufacturer")
        {
        }
    }
}