using PDAB.Models;

namespace PDAB.ViewModels
{
    public class NewEmployeeViewModel : SingleEntityViewModel<Employee>
    {
        public NewEmployeeViewModel() : base("Employee")
        {
            item = new Employee();
        }

        public string FirstName
        {
            get => item.FirstName;
            set
            {
                item.FirstName = value;
                OnPropertyChanged(() => FirstName);
            }
        }

        public string LastName
        {
            get => item.LastName;
            set
            {
                item.LastName = value;
                OnPropertyChanged(() => LastName);
            }
        }

        public string Position
        {
            get => item.Position;
            set
            {
                item.Position = value;
                OnPropertyChanged(() => Position);
            }
        }

        public string Email
        {
            get => item.Email;
            set
            {
                item.Email = value;
                OnPropertyChanged(() => Email);
            }
        }

        public string Phone
        {
            get => item.Phone;
            set
            {
                item.Phone = value;
                OnPropertyChanged(() => Phone);
            }
        }

        public override void Save()
        {
            dbContext.Employees.Add(item);
            dbContext.SaveChanges();
        }
    }
}