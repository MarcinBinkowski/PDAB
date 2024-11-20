using System.Collections.ObjectModel;
using PDAB.Models;

namespace PDAB.ViewModels
{
public class AllRolesViewModel : BaseDataViewModel<Role>
{
    public AllRolesViewModel(IRepository<Role> repository) 
        : base(repository, "Roles")
    {
    }
}
}