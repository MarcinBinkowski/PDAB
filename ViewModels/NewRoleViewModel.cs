using Microsoft.EntityFrameworkCore;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class NewRoleViewModel : SingleEntityViewModel<Role>
    {
        #region Constructor
        public NewRoleViewModel()
            : base("Role")
        {
            item = new Role();
        }
        #endregion

        #region Properties

        public string RoleName
        {
            get
            {
                return item.RoleName;
            }
            set
            {
                item.RoleName = value;
                OnPropertyChanged(() => RoleName);
            }
        }

        public string? Description
        {
            get
            {
                return item.Description;
            }
            set
            {
                item.Description = value;
                OnPropertyChanged(() => Description);
            }
        }
        #endregion

        #region Helpers
        public override bool Save()
        {
            try
            {
                dbContext.Roles.Add(item);
                dbContext.SaveChanges();
                return true;
            }
            catch (DbUpdateException exception)
            {
                Console.WriteLine(exception);
                if (exception.InnerException != null)
                {
                    Console.WriteLine(exception.InnerException.Message);
                }
                return false;
            }
        }
        #endregion
    }
}