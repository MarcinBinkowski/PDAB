using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace PDAB.Helpers
{
    public class NameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string name = value?.ToString();
        
            if (string.IsNullOrEmpty(name))
                return ValidationResult.ValidResult;

            if (name.Any(char.IsDigit))
                return new ValidationResult(false, "Name cannot contain numbers");

            if (name.Any(c => !char.IsLetter(c) && !char.IsWhiteSpace(c)))
                return new ValidationResult(false, "Name can only contain letters");

            return ValidationResult.ValidResult;
        }
    }
}