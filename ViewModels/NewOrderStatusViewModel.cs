using Microsoft.EntityFrameworkCore;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class NewOrderStatusViewModel : SingleEntityViewModel<OrderStatus>
    {
        public NewOrderStatusViewModel() : base("Order Status")
        {
            item = new OrderStatus();
        }

        public string StatusName
        {
            get => item.StatusName;
            set
            {
                item.StatusName = value;
                OnPropertyChanged(() => StatusName);
            }
        }

        public string? Description
        {
            get => item.Description;
            set
            {
                item.Description = value;
                OnPropertyChanged(() => Description);
            }
        }

        public override bool Save()
        {
            try
            {
                dbContext.OrderStatuses.Add(item);
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
