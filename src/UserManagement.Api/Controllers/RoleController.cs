using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Api.Common;
using UserManagement.Application.Common.Models.Dto;
using UserManagement.Application.Role.Commands.CreateRole;
using UserManagement.Application.Role.Commands.DeleteRole;
using UserManagement.Application.Role.Queries.GetRoles;
using UserManagement.Domain.Common.Models;
using UserManagement.Infrastructure.Helpers;

namespace UserManagement.Api.Controllers;

[Route("api/v{version:apiVersion}/roles")]
[ApiVersion("1.0")]
public class RoleController(IMediator mediator) : ApiControllerBase(mediator)
{
    /// <summary>
    /// Creates a new role.
    /// </summary>
    /// <remarks>
    /// This endpoint allows you to create a new role in the system.
    ///
    /// Sample request:
    ///
    ///     POST /api/v1/roles
    ///     {
    ///         "RoleName": "SampleRole"
    ///     }
    ///
    /// </remarks>
    /// <param name="model">The model for creating a role.</param>
    /// <param name="cancellationToken">The cancellation token for the request.</param>
    /// <returns>The created role details.</returns>
    /// <response code="200">The role was created successfully.</response>
    /// <response code="400">The request model is invalid</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have permission to create role.</response>
    /// <response code="409">A role with the same name already exists.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpPost]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<Result<RoleDto>>> CreateRoleAsync([FromBody] CreateRoleCommandModel model, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new CreateRoleCommand { Model = model }, cancellationToken);
        
        return ResponseHelper.CreateResponse(response);
    }
    
    /// <summary>
    /// Retrieves a list of all roles.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token for the request.</param>
    /// <returns>The list of roles.</returns>
    /// <response code="200">The list of roles was retrieved successfully.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have permission to access roles.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpGet]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<Result<IEnumerable<RoleDto>>>> GetRolesAsync(CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new GetRolesQuery(), cancellationToken);
        
        return ResponseHelper.CreateResponse(response);
    }
    
    /// <summary>
    /// Deletes a role by name.
    /// </summary>
    /// <param name="roleName">The name of the role to be deleted.</param>
    /// <param name="cancellationToken">The cancellation token for the request.</param>
    /// <returns>The command result indicating the status of the operation.</returns>
    /// <response code="200">The role was deleted successfully.</response>
    /// <response code="400">The request is invalid.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have permission to delete roles.</response>
    /// <response code="404">The role was not found.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpDelete("{roleName}")]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<Result<string>>> DeleteRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new DeleteRoleCommand { RoleName = roleName }, cancellationToken);
        
        return ResponseHelper.CreateResponse(response);
    }
}