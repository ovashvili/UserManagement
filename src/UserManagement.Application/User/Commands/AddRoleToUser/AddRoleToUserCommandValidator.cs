using FluentValidation;

namespace UserManagement.Application.User.Commands.AddRoleToUser;

public class AddRoleToUserCommandValidator : AbstractValidator<AddRoleToUserCommand>
{
    public AddRoleToUserCommandValidator()
    {
        _ = RuleFor(x => x.UserId)
            .NotEmpty();

        _ = RuleFor(x => x.Model.RoleName)
            .NotEmpty();
    }
}