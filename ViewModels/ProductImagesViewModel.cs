namespace PDAB.ViewModels;

public class ProductImagesViewModel : ViewModelBase
{
    private ObservableCollection<BitmapImage> _productImages;
    private readonly IProductService _productService;

    public ObservableCollection<BitmapImage> ProductImages
    {
        get => _productImages;
        set
        {
            _productImages = value;
            OnPropertyChanged();
        }
    }

    public ProductImagesViewModel(IProductService productService)
    {
        _productService = productService;
        _productImages = new ObservableCollection<BitmapImage>();
        LoadImagesAsync();
    }

    private async Task LoadImagesAsync()
    {
        var products = await _productService.GetAllProducts();
        foreach (var product in products)
        {
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                try
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri(product.ImageUrl);
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();
                    ProductImages.Add(image);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error loading image: {ex.Message}");
                }
            }
        }
    }
}