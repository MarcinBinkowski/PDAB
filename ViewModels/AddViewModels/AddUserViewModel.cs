using System.Collections.ObjectModel;
using System.Windows.Input;
using PDAB.Helpers;
using PDAB.Models;
using PDAB.Repository;
using PDAB.Services;

namespace PDAB.ViewModels
{
    public class AddUserViewModel : BaseAddViewModel<User>
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly DialogService _dialogService;
        private readonly PasswordService _passwordService;

        private ObservableCollection<Employee> _employees;
        private ObservableCollection<Role> _roles;
        
        private string _password;

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                if (!string.IsNullOrEmpty(value))
                {
                    Entity.PasswordHash = _passwordService.HashPassword(value);
                }
                OnPropertyChanged(nameof(Password));
            }
        }

        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set
            {
                _employees = value;
                OnPropertyChanged(nameof(Employees));
            }
        }

        public ObservableCollection<Role> Roles
        {
            get => _roles;
            set
            {
                _roles = value;
                OnPropertyChanged(nameof(Roles));
            }
        }


        public AddUserViewModel(
            IRepositoryFactory repositoryFactory,
            DialogService dialogService,
            PasswordService passwordService) 
            : base(repositoryFactory.GetRepository<User>(), "Add User")
        {
            _repositoryFactory = repositoryFactory;
            _dialogService = dialogService;
            _passwordService = passwordService;
            LoadData();
        }

        private async void LoadData()
        {
            Employees = await _repositoryFactory.GetRepository<Employee>().GetAllAsync();
            Roles = await _repositoryFactory.GetRepository<Role>().GetAllAsync();
        }

        public ICommand SelectEmployeeCommand => new BaseCommand(async () => 
        {
            var selected = await _dialogService.ShowSelectionDialog("Select Employee", Employees);
            if (selected is Employee employee)
            {
                Entity.Employee = employee;
                Entity.EmployeeId = employee.EmployeeId;
                OnPropertyChanged(nameof(Entity));
            }
        });

        public ICommand SelectRoleCommand => new BaseCommand(async () => 
        {
            var selected = await _dialogService.ShowSelectionDialog("Select Role", Roles);
            if (selected is Role role)
            {
                Entity.Role = role;
                Entity.RoleId = role.RoleId;
                OnPropertyChanged(nameof(Entity));
            }
        });
    }}