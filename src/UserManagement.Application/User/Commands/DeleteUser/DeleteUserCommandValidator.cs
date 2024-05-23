using FluentValidation;

namespace UserManagement.Application.User.Commands.DeleteUser;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        _ = RuleFor(x => x.Id)
            .NotEmpty();
    }
}