using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UserManagement.Application.Common.Models;

namespace UserManagement.Application.User.Commands.AuthenticateUser;

public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, Result<AuthenticateUserResponse>>
{
    public Task<Result<AuthenticateUserResponse>> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}