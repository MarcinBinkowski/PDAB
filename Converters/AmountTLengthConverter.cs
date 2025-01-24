using System;
using System.Globalization;
using System.Windows.Data;

namespace PDAB.Converters
{
    public class AmountTLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal amount)
            {
                var scaledValue = (double)amount / 100.0;

                if (scaledValue > 500)
                {
                    return 500;
                }

                return scaledValue;
            }
            return 0;
        }
        

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}