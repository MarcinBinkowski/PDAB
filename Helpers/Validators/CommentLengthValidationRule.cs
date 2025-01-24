using System.Globalization;
using System.Windows.Controls;

namespace PDAB.Helpers
{
    public class CommentLengthValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string comment = value?.ToString();
        
            if (string.IsNullOrWhiteSpace(comment))
                return ValidationResult.ValidResult;

            if (comment.Length < 5)
                return new ValidationResult(false, "Comment must be at least 5 characters long");

            return ValidationResult.ValidResult;
        }
    }
}