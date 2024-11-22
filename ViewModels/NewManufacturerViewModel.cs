using PDAB.Models;

namespace PDAB.ViewModels
{
    public class NewManufacturerViewModel : SingleEntityViewModel<Manufacturer>
    {
        public NewManufacturerViewModel() : base("Manufacturer")
        {
            item = new Manufacturer();
        }

        public string ManufacturerName
        {
            get => item.ManufacturerName;
            set
            {
                item.ManufacturerName = value;
                OnPropertyChanged(() => ManufacturerName);
            }
        }

        public override void Save()
        {
            dbContext.Manufacturers.Add(item);
            dbContext.SaveChanges();
        }
    }
}