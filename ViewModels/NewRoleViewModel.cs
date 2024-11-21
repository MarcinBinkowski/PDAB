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
        public int RoleId
        {
            get
            {
                return item.RoleId;
            }
            set
            {
                item.RoleId = value;
                OnPropertyChanged(() => RoleId);
            }
        }

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
        public override void Save()
        {
            dbContext.Roles.Add(item);
            dbContext.SaveChanges();
        }
        #endregion
    }
}