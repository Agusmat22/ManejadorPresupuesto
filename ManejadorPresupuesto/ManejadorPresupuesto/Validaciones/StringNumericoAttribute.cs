using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ManejadorPresupuesto.Validaciones
{
    public class StringNumericoAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            if (!Regex.IsMatch(value.ToString(), "^[0-9]+$"))
            {
                return new ValidationResult("Debe ingresar una cadena numerica");

            }

            return ValidationResult.Success;
        }
        

    }
}
