using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
using StatusCodes = UserManagement.Domain.Common.Enums.StatusCodes;

namespace UserManagement.Api.Controllers;

[Route("api/v{version:apiVersion}/users")]
[ApiVersion("1.0")]
public class UserController(IMediator mediator) : ApiControllerBase(mediator)
{
    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <remarks>
    /// This endpoint allows you to register a new user in the system.
    ///
    /// Sample request:
    ///
    ///     POST /api/v1/users
    ///     {
    ///         "firstName": "John",
    ///         "lastName": "Doe",
    ///         "userName": "johndoe",
    ///         "email": "johndoe@example.com",
    ///         "password": "P@ssw0rd123"
    ///     }
    ///
    /// </remarks>
    /// <param name="model">The request model containing the user details.</param>
    /// <param name="cancellationToken">The cancellation token for the request.</param>
    /// <returns>The registered user.</returns>
    /// <response code="200">The user was registered successfully.</response>
    /// <response code="400">The request model is invalid.</response>
    /// <response code="409">A user with the same username or email already exists.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpPost]
    public async Task<ActionResult<Result<UserDto>>> RegisterUserAsync([FromBody] RegisterUserCommandModel model,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new RegisterUserCommand { Model = model }, cancellationToken);
        return ResponseHelper.CreateResponse(result);
    }

    /// <summary>
    /// Retrieves a user by ID.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve.</param>
    /// <param name="cancellationToken">The cancellation token for the request.</param>
    /// <returns>The user details.</returns>
    /// <response code="200">The user was retrieved successfully.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have permission to access the resource.</response>
    /// <response code="404">The user was not found.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpGet("{userId:guid}")]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<Result<UserDto>>> GetUserAsync([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetUserByIdQuery { Id = userId }, cancellationToken);
        return ResponseHelper.CreateResponse(result);
    }
    
    /// <summary>
    /// Retrieves a list of all users.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token for the request.</param>
    /// <returns>The list of users.</returns>
    /// <response code="200">The list of users was retrieved successfully.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have permission to access the resource.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpGet]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<Result<IEnumerable<UserDto>>>> GetUsersAsync(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetUsersQuery(), cancellationToken);
        return ResponseHelper.CreateResponse(result);
    }
    
    /// <summary>
    /// Updates a user's details.
    /// </summary>
    /// <param name="userId">The ID of the user to update.</param>
    /// <param name="model">The request model containing the updated user details.</param>
    /// <param name="cancellationToken">The cancellation token for the request.</param>
    /// <returns>The updated user details.</returns>
    /// <response code="200">The user was updated successfully.</response>
    /// <response code="400">The request model is invalid.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have permission to update the resource.</response>
    /// <response code="404">The user was not found.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpPut("{userId:guid}")]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<Result<UserDto>>> UpdateUserAsync([FromRoute] Guid userId,
        [FromBody] UpdateUserCommandModel model, CancellationToken cancellationToken)
    {
        if(!HttpContext.HasPermission(userId))
            return ResponseHelper.CreateCustomErrorResult(
                Result.Failed("You do not have permission to update this user.", StatusCodes.Forbidden), 403);
        
        var result = await Mediator.Send(new UpdateUserCommand { Id = userId, Model = model }, cancellationToken);
        return ResponseHelper.CreateResponse(result);
    }
    
    /// <summary>
    /// Authenticates a user and returns an access token.
    /// </summary>
    /// <param name="model">The request model containing the user credentials.</param>
    /// <param name="cancellationToken">The cancellation token for the request.</param>
    /// <returns>The authentication response containing the access token.</returns>
    /// <response code="200">The user was authenticated successfully,and an access token was generated.</response>
    /// <response code="400">The request model is invalid.</response>
    /// <response code="401">The user credentials are invalid.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpPost("authenticate")]
    public async Task<ActionResult<Result<AuthenticateUserResponse>>> AuthenticateUserAsync(
        [FromBody] AuthenticateUserCommandModel model, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId();
        var result = await Mediator.Send(new AuthenticateUserCommand { UserId = userId, Model = model },
            cancellationToken);
        return ResponseHelper.CreateResponse(result);
    }
    
    /// <summary>
    /// Deletes a user by ID.
    /// </summary>
    /// <param name="userId">The ID of the user to delete.</param>
    /// <param name="cancellationToken">The cancellation token for the request.</param>
    /// <returns>A success message indicating the user was deleted.</returns>
    /// <response code="200">The user was deleted successfully.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have permission to delete the resource.</response>
    /// <response code="404">The user was not found.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpDelete("{userId:guid}")]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<Result<string>>> DeleteUserAsync([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeleteUserCommand { Id = userId }, cancellationToken);
        return ResponseHelper.CreateResponse(result);
    }
    
    /// <summary>
    /// Removes a role from a user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="roleName">The name of the role to remove.</param>
    /// <param name="cancellationToken">The cancellation token for the request.</param>
    /// <returns>The command result indicating the status of the operation.</returns>
    /// <response code="200">The role was removed from the user successfully.</response>
    /// <response code="400">The request is invalid.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have permission to modify the resource.</response>
    /// <response code="404">The user or role was not found.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpDelete("{userId:guid}/role/{roleName}")]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<Result<string>>> RemoveRoleFromUserAsync([FromRoute] Guid userId,
        [FromRoute] string roleName, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new RemoveRoleFromUserCommand { UserId = userId, RoleName = roleName },
            cancellationToken);
        return ResponseHelper.CreateResponse(result);
    }
    
    /// <summary>
    /// Retrieves the roles assigned to a user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="cancellationToken">The cancellation token for the request.</param>
    /// <returns>The list of roles assigned to the user.</returns>
    /// <response code="200">The roles were retrieved successfully.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have permission to access the resource.</response>
    /// <response code="404">The user was not found.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpGet("{userId:guid}/roles")]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<Result<IEnumerable<RoleDto>>>> GetUserRolesAsync([FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetUserRolesQuery { UserId = userId }, cancellationToken);
        return ResponseHelper.CreateResponse(result);
    }
    
    /// <summary>
    /// Assigns a role to a user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="model">The request model containing the role details.</param>
    /// <param name="cancellationToken">The cancellation token for the request.</param>
    /// <returns>The command result indicating the status of the operation.</returns>
    /// <response code="200">The role was assigned to the user successfully.</response>
    /// <response code="400">The request model is invalid.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have permission to modify the resource.</response>
    /// <response code="404">The user or role was not found.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpPost("{userId:guid}/role")]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<Result<string>>> AddRoleToUserAsync([FromRoute] Guid userId,
        [FromBody] AddRoleToUserCommandModel model, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new AddRoleToUserCommand { UserId = userId,  Model = model },
            cancellationToken);
        return ResponseHelper.CreateResponse(result);
    }
}