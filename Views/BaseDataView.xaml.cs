using System.Windows.Controls;
using System.Windows.Input;
using PDAB.ViewModels;

namespace PDAB.Views
{
    public partial class BaseDataView : UserControl
    {
        public BaseDataView()
        {
            InitializeComponent();
        }
        private async void DataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var dataGrid = sender as DataGrid;
                if (dataGrid?.SelectedItem != null && DataContext is IDeletable deletable)
                {
                    await deletable.DeleteItemAsync(dataGrid.SelectedItem);
                }
            }
        }
    }
}