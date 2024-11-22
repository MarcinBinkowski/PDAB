using System.Collections.ObjectModel;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class AllDiscountsViewModel : AllEntitiesViewModel<Discount>
    {
        public AllDiscountsViewModel() : base("Discounts")
        {
            Load();
        }

        public override void Load()
        {
            List = new ObservableCollection<Discount>(
                dbContext.Discounts.ToList()
            );
        }
    }
}