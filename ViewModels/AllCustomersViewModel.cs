using System.Collections.ObjectModel;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class AllCustomersViewModel : BaseWorkspaceViewModel
    {
        private readonly IRepository<Customer> _customerRepository;
        private ObservableCollection<Customer> _customers;

        public ObservableCollection<Customer> Customers
        {
            get => _customers;
            set
            {
                _customers = value;
                OnPropertyChanged(nameof(Customers));
            }
        }

        public AllCustomersViewModel(IRepository<Customer> customerRepository)
        {
            DisplayName = "Customers";
            _customerRepository = customerRepository;
            LoadCustomers();
        }

        private async void LoadCustomers()
        {
            Customers = await _customerRepository.GetAllAsync();
        }
    }
}