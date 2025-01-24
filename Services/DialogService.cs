using System.Collections.ObjectModel;
using MaterialDesignThemes.Wpf;
using PDAB.ViewModels;
using PDAB.Views;

public class DialogService
{
    public async Task<T?> ShowSelectionDialog<T>(string title, IEnumerable<T> items) where T : class
    {
        Console.WriteLine($"Opening dialog for {typeof(T).Name}");
        
        var viewModel = new SelectionDialogViewModel<T>(items, title);
        var view = new SelectionDialogView { DataContext = viewModel };
        
        var result = await DialogHost.Show(view, "RootDialog");
        Console.WriteLine($"Dialog closed with result: {result}");
        
        return result as T;
    }
}