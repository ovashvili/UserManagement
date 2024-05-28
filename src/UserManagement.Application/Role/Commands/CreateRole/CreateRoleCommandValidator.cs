using FluentValidation;

namespace UserManagement.Application.Role.Commands.CreateRole;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        _ = RuleFor(x => x.Model.RoleName)
            .NotEmpty()
            .MaximumLength(25);
    }
}