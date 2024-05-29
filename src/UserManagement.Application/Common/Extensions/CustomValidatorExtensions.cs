using FluentValidation;

namespace UserManagement.Application.Common.Extensions;

public static class CustomValidatorExtensions
{
    public static IRuleBuilderOptions<T, string> ValidatePasswordRule<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Your password cannot be empty")
            .MinimumLength(8).WithMessage("Your password length must be at least 8.")
            .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
            .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
            .Matches(@"[^\w\s]+").WithMessage("Your password must contain one or more special characters.") // This regex matches any special character
            .Matches("^[^ ]*$").WithMessage("Your password must not contain spaces.");
    }
}