using FluentValidation;

namespace UserManagement.Application.User.Commands.AuthenticateUser;

public class AuthenticateUserCommandValidator : AbstractValidator<AuthenticateUserCommand>
{
    public AuthenticateUserCommandValidator()
    {
    }
}