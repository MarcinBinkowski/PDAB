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
            if (e.Key == Key.Delete && DataContext is BaseWorkspaceViewModel viewModel)
            {
                var method = viewModel.GetType().GetMethod("DeleteSelectedItem");
                if (method != null)
                {
                    await (Task)method.Invoke(viewModel, null);
                }
            }
        }
    }
}