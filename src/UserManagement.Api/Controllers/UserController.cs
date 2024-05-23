using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Api.Common;
using UserManagement.Application.Common.Models;
using UserManagement.Application.User.Commands.AuthenticateUser;

namespace UserManagement.Api.Controllers;

[Route("/user")]
[ApiVersion("1.0")]
public class UserController(IMediator mediator) : ApiControllerBase(mediator)
{
    [HttpPost("authenticate")]
    public async Task<ActionResult<Result<AuthenticateUserResponse>>> AuthenticateUserAsync(
        [FromBody] AuthenticateUserCommandModel model, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new AuthenticateUserCommand { Model = model }, cancellationToken));
    }
}