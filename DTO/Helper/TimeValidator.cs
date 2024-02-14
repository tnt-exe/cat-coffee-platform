using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DTO.Helper
{
    public class TimeValidatorAttribute : ValidationAttribute
    {
        private readonly string? _format;

        public TimeValidatorAttribute(string? format = "HH:mm")
        {
            _format = format;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (TimeOnly.TryParseExact(value.ToString(), _format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return ValidationResult.Success!;
            }

            return new ValidationResult($"Time {validationContext.MemberName} must be a time in the format {_format}.");
        }
    }
}
