using System.Collections.ObjectModel;
using System.Windows.Input;
using PDAB.Helpers;
using PDAB.Models;
using PDAB.Repository;

namespace PDAB.ViewModels
{
public class AddProductViewModel : BaseAddViewModel<Product>
{
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IDialogService _dialogService;
    private ObservableCollection<Category> _categories;
    private ObservableCollection<Manufacturer> _manufacturers;

    public ObservableCollection<Category> Categories
    {
        get => _categories;
        set
        {
            _categories = value;
            OnPropertyChanged(nameof(Categories));
        }
    }

    public ObservableCollection<Manufacturer> Manufacturers
    {
        get => _manufacturers;
        set
        {
            _manufacturers = value;
            OnPropertyChanged(nameof(Manufacturers));
        }
    }

    public AddProductViewModel(IRepositoryFactory repositoryFactory, IDialogService dialogService) 
        : base(repositoryFactory.GetRepository<Product>(), "Add Product")
    {
        _repositoryFactory = repositoryFactory;
        _dialogService = dialogService;
        LoadData();
    }

    private async void LoadData()
    {
        Categories = await _repositoryFactory.GetRepository<Category>().GetAllAsync();
        Manufacturers = await _repositoryFactory.GetRepository<Manufacturer>().GetAllAsync();
    }

    public ICommand SelectCategoryCommand => new BaseCommand(async () => 
    {
        var selected = await _dialogService.ShowSelectionDialog("Select Category", Categories);
        if (selected is Category category)
        {
            Entity.Category = category;
            Entity.CategoryId = category.CategoryId;
            OnPropertyChanged(nameof(Entity));
        }
    });

    public ICommand SelectManufacturerCommand => new BaseCommand(async () => 
    {
        var selected = await _dialogService.ShowSelectionDialog("Select Manufacturer", Manufacturers);
        if (selected is Manufacturer manufacturer)
        {
            Entity.Manufacturer = manufacturer;
            Entity.ManufacturerId = manufacturer.ManufacturerId;
            OnPropertyChanged(nameof(Entity));
        }
    });
}
}