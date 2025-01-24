using System.Globalization;
using System.Windows.Controls;

namespace PDAB.Helpers
{
    public class RatingValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!byte.TryParse(value?.ToString(), out byte rating))
                return new ValidationResult(false, "Rating must be a number");

            if (rating < 1 || rating > 5)
                return new ValidationResult(false, "Rating must be between 1 and 5");

            return ValidationResult.ValidResult;
        }
    }
}