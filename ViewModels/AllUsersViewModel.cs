using System.Collections.ObjectModel;
using PDAB.Models;

namespace PDAB.ViewModels
{
public class AllUsersViewModel : BaseDataViewModel<User>
{
    public AllUsersViewModel(IRepository<User> repository) 
        : base(repository, "Users")
    {
    }
}
}