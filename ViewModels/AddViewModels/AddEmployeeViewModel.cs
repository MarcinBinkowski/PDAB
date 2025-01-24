using System.Windows.Input;
using PDAB.Helpers;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class AddEmployeeViewModel : BaseAddViewModel<Employee>
    {
        public AddEmployeeViewModel(IRepository<Employee> repository) 
            : base(repository, "Add Employee")
        {
        }
    }
}