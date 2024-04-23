using FluentValidation.Results;

namespace ADIA.Service.Validations.Extensions.Extensions;

public static class ValidatorExtensions
{
    public static List<string> GetMessageErrors(this ValidationResult validator)
    {
        if (validator.Errors.Any())
        {
            return validator.Errors.Select(error => error.ErrorMessage).OrderBy(msg => msg).ToList();
        }

        return new List<string>();
    }
}