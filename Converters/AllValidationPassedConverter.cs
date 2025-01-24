
using System.Globalization;
using System.Windows.Data;

namespace PDAB.Converters
{
    public class AllValidationPassedConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.All(x => !(x is bool) || !(bool)x);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}