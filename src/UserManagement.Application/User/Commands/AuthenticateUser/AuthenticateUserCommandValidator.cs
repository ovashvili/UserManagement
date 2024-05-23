using FluentValidation;

namespace UserManagement.Application.User.Commands.AuthenticateUser;

public class AuthenticateUserCommandValidator : AbstractValidator<AuthenticateUserCommand>
{
    public AuthenticateUserCommandValidator()
    {
        _ = RuleFor(x => x.Model.Username)
            .NotEmpty();

        _ = RuleFor(x => x.Model.Password)
            .NotEmpty();
    }
}