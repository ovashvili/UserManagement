using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Api.Common;
using UserManagement.Api.Extensions;
using UserManagement.Application.Common.Models.Dto;
using UserManagement.Application.User.Commands.AddRoleToUser;
using UserManagement.Application.User.Commands.AuthenticateUser;
using UserManagement.Application.User.Commands.DeleteUser;
using UserManagement.Application.User.Commands.RegisterUser;
using UserManagement.Application.User.Commands.RemoveRoleFromUser;
using UserManagement.Application.User.Commands.UpdateUser;
using UserManagement.Application.User.Queries.GetUserById;
using UserManagement.Application.User.Queries.GetUserRoles;
using UserManagement.Application.User.Queries.GetUsers;
using UserManagement.Domain.Common.Models;
using UserManagement.Infrastructure.Helpers;

namespace UserManagement.Api.Controllers;

[Route("api/v{version:apiVersion}/users")]
[ApiVersion("1.0")]
public class UserController(IMediator mediator) : ApiControllerBase(mediator)
{
    [HttpPost]
    public async Task<ActionResult<Result<UserDto>>> RegisterUserAsync([FromBody] RegisterUserCommandModel model,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new RegisterUserCommand { Model = model }, cancellationToken);
        return ResponseHelper.CreateResponse(result);
    }

    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<Result<UserDto>>> GetUserAsync([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetUserByIdQuery { Id = userId }, cancellationToken);
        return ResponseHelper.CreateResponse(result);
    }
    
    [HttpGet]
    public async Task<ActionResult<Result<IEnumerable<UserDto>>>> GetUsersAsync(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetUsersQuery(), cancellationToken);
        return ResponseHelper.CreateResponse(result);
    }
    
    [HttpPut("{userId:guid}")]

    public async Task<ActionResult<Result<UserDto>>> UpdateUserAsync([FromRoute] Guid userId,
        [FromBody] UpdateUserCommandModel model, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new UpdateUserCommand { Id = userId, Model = model }, cancellationToken);
        return ResponseHelper.CreateResponse(result);
    }
    
    [HttpPost("authenticate")]
    public async Task<ActionResult<Result<AuthenticateUserResponse>>> AuthenticateUserAsync(
        [FromBody] AuthenticateUserCommandModel model, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId();
        var result = await Mediator.Send(new AuthenticateUserCommand { UserId = userId, Model = model },
            cancellationToken);
        return ResponseHelper.CreateResponse(result);
    }
    
    [HttpDelete("{userId:guid}")]
    public async Task<ActionResult<Result<string>>> DeleteUserAsync([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeleteUserCommand { Id = userId }, cancellationToken);
        return ResponseHelper.CreateResponse(result);
    }
    
    [HttpDelete("{userId:guid}/role/{roleName}")]
    public async Task<ActionResult<Result<string>>> RemoveRoleFromUserAsync([FromRoute] Guid userId,
        [FromRoute] string roleName, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new RemoveRoleFromUserCommand { UserId = userId, RoleName = roleName },
            cancellationToken);
        return ResponseHelper.CreateResponse(result);
    }
    
    [HttpGet("{userId:guid}/roles")]
    public async Task<ActionResult<Result<IEnumerable<RoleDto>>>> GetUserRolesAsync([FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetUserRolesQuery { UserId = userId }, cancellationToken);
        return ResponseHelper.CreateResponse(result);
    }
    
    [HttpPost("{userId:guid}/role")]
    public async Task<ActionResult<Result<string>>> AddRoleToUserAsync([FromRoute] Guid userId,
        [FromBody] AddRoleToUserCommandModel model, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new AddRoleToUserCommand { UserId = userId,  Model = model },
            cancellationToken);
        return ResponseHelper.CreateResponse(result);
    }
}