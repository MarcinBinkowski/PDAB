using System.Collections.ObjectModel;

public interface IDialogService
{
    Task<T?> ShowSelectionDialog<T>(string title, IEnumerable<T> items) where T : class;
}