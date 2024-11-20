
namespace PDAB.ViewModels
{
    public class SelectionDialogViewModel<T> : BaseViewModel where T : class
    {
        public string Title { get; }
        public IEnumerable<T> Items { get; }
        public bool HasSelection => SelectedItem != null;
        private T _selectedItem;
        public T SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
                OnPropertyChanged(nameof(HasSelection));
            }
        }

        public SelectionDialogViewModel(IEnumerable<T> items, string title)
        {
            Items = items;
            Title = title;
        }
    }
}
