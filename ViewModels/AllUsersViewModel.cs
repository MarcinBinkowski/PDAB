using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class AllUsersViewModel : AllEntitiesViewModel<User>
    {
        public AllUsersViewModel() : base("Users")
        {
            Load();
        }

        public override void Load()
        {
            List = new ObservableCollection<User>(
                dbContext.Users
                    .Include(u => u.Employee)
                    .Include(u => u.Role)
                    .ToList()
            );
        }
    }
}