using System.Collections.ObjectModel;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class AllEmployeesViewModel : AllEntitiesViewModel<Employee>
    {
        public AllEmployeesViewModel() : base("Employees")
        {
            Load();
        }

        public override void Load()
        {
            List = new ObservableCollection<Employee>(
                dbContext.Employees.ToList()
            );
        }
    }
}