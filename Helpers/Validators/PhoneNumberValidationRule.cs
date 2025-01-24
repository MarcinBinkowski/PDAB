using System.Globalization;
using System.Windows.Controls;


namespace PDAB.Helpers
{
    public class PhoneNumberValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string phoneNumber = value?.ToString();
            if (string.IsNullOrEmpty(phoneNumber))
                return ValidationResult.ValidResult;

            if (!phoneNumber.All(char.IsDigit))
                return new ValidationResult(false, "Phone number can only contain digits");

            return ValidationResult.ValidResult;
        }
    }   
}

