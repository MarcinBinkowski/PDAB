using System.Collections.ObjectModel;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class AllEmployeesViewModel : BaseDataViewModel<Employee>
    {
        public AllEmployeesViewModel(IRepository<Employee> repository) 
            : base(repository, "Employees")
        {
        }
    }
}