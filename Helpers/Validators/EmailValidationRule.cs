using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace PDAB.Helpers
{
    public class EmailValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string email = value?.ToString();
        
            if (string.IsNullOrEmpty(email))
                return ValidationResult.ValidResult;

            if (!email.Contains("@") || !email.Contains("."))
                return new ValidationResult(false, "Invalid email format");

            return ValidationResult.ValidResult;
        }
    }
}