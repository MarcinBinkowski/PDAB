
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace PDAB.Converters
{
    public class EnumToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Console.WriteLine($"Converting: value={value}, parameter={parameter}");
        
            if (value == null || parameter == null)
                return false;
            
            return value.ToString() == parameter.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Console.WriteLine($"Converting back: value={value}, parameter={parameter}");
        
            if (!(value is bool boolValue) || !boolValue)
                return null;
            
            return Enum.Parse(targetType, parameter.ToString());
        }
    }
}