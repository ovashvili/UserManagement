using FluentValidation;

namespace UserManagement.Application.Role.Commands.DeleteRole;

public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator()
    {
        _ = RuleFor(x => x.RoleName)
            .NotEmpty();
    }
}