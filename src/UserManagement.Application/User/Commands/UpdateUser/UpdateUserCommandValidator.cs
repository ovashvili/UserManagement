using FluentValidation;
using UserManagement.Application.Common.Extensions;

namespace UserManagement.Application.User.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        _ = RuleFor(x => x.Id)
            .NotEmpty();

        _ = RuleFor(x => x.Model.UserName)
            .NotEmpty().WithMessage("Your username cannot be empty")
            .MaximumLength(20).WithMessage("Your username length must not exceed 20.");

        _ = RuleFor(x => x.Model.FirstName)
            .NotEmpty().WithMessage("Your firstname cannot be empty")
            .MaximumLength(60).WithMessage("Your username length must not exceed 60.");

        _ = RuleFor(x => x.Model.LastName)
            .NotEmpty().WithMessage("Your lastname cannot be empty")
            .MaximumLength(120).WithMessage("Your username length must not exceed 120.");

        _ = RuleFor(p => p.Model.Password)
            .ValidatePasswordRule();
    }
}