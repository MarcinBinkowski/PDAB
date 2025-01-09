using System.Windows;
using Microsoft.EntityFrameworkCore;
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

        public override bool Save()
        {
            try
            {
                dbContext.Manufacturers.Add(item);
                dbContext.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }
    }
}