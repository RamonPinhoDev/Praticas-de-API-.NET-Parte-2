using System.ComponentModel.DataAnnotations;

namespace APICatologo.Validation
{
    public class PrimeiraLetraMaiusculaAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {

                return ValidationResult.Success;

            }

            var primeiraLetra = value.ToString()[0].ToString();

            if (primeiraLetra != primeiraLetra.ToUpper())
            {


                return new ValidationResult("A primeira letra tem que ser munuscula");
            }

            return ValidationResult.Success;
        }
    }
}
