using System.Collections;
using System.Text.RegularExpressions;
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
        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (ShouldSkipProperty(e.PropertyType))
            {
                e.Cancel = true;
                return;
            }
            e.Column.Header = FormatColumnHeader(e.PropertyName);
        }

        private bool ShouldSkipProperty(Type propertyType)
        {
            if (typeof(IEnumerable).IsAssignableFrom(propertyType) && propertyType != typeof(string))
                return true;

            if (!propertyType.IsPrimitive 
                && !propertyType.IsEnum
                && propertyType != typeof(string)
                && !propertyType.IsValueType)
                return true;

            return false;
        }

        private string FormatColumnHeader(string propertyName)
        {
            if (propertyName.EndsWith("Id"))
                propertyName = propertyName[..^2];

            return Regex.Replace(propertyName, "([A-Z])", " $1").Trim();
        }
    }
}