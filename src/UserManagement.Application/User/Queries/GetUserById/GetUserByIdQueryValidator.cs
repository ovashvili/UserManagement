using FluentValidation;

namespace UserManagement.Application.User.Queries.GetUserById;

public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        _ = RuleFor(x => x.Id)
            .NotEmpty();
    }
}