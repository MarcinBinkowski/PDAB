using System.Windows.Input;
using PDAB.Helpers;
using PDAB.Models;

namespace PDAB.ViewModels
{
public class AddRoleViewModel : BaseAddViewModel<Role>
    {
        public AddRoleViewModel(IRepository<Role> repository)
            : base(repository, "Add Role")
        {
        }
    }
}