using System.Windows.Input;
using PDAB.Helpers;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class AddCustomerViewModel : BaseAddViewModel<Customer>
    {
        private bool _isPhoneValid = true;

        public bool IsPhoneValid
        {
            get => _isPhoneValid;
            set
            {
                _isPhoneValid = value;
                OnPropertyChanged(nameof(IsPhoneValid));
                (SaveCommand as BaseCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ICommand SaveCommand => _saveCommand ??= new BaseCommand(
            execute: async () => await SaveAsync(),
            canExecute: () => IsPhoneValid
        );

        public AddCustomerViewModel(IRepository<Customer> repository) 
            : base(repository, "Add Customer")
        {
        }
    }
}
