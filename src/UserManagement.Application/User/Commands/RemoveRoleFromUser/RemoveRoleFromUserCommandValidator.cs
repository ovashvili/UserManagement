using FluentValidation;

namespace UserManagement.Application.User.Commands.RemoveRoleFromUser;

public class RemoveRoleFromUserCommandValidator : AbstractValidator<RemoveRoleFromUserCommand>
{
    public RemoveRoleFromUserCommandValidator()
    {
        _ = RuleFor(x => x.UserId)
            .NotEmpty();

        _ = RuleFor(x => x.RoleName)
            .NotEmpty();
    }
}