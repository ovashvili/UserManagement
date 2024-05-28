using FluentValidation;

namespace UserManagement.Application.User.Queries.GetUserRoles;

public class GetUserRolesQueryValidator : AbstractValidator<GetUserRolesQuery>
{
    public GetUserRolesQueryValidator()
    {
        _ = RuleFor(x => x.UserId)
            .NotEmpty();
    }
}