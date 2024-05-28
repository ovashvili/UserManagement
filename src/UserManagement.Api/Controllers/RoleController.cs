using Asp.Versioning;
using MediatR;
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
    [HttpPost]
    public async Task<ActionResult<Result<string>>> CreateRoleAsync([FromBody] CreateRoleCommandModel model, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new CreateRoleCommand { Model = model }, cancellationToken);
        
        return ResponseHelper.CreateResponse(response);
    }
    
    [HttpGet]
    public async Task<ActionResult<Result<IEnumerable<RoleDto>>>> GetRolesAsync(CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new GetRolesQuery(), cancellationToken);
        
        return ResponseHelper.CreateResponse(response);
    }
    
    [HttpDelete("{roleName}")]
    public async Task<ActionResult<Result<string>>> DeleteRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new DeleteRoleCommand { RoleName = roleName }, cancellationToken);
        
        return ResponseHelper.CreateResponse(response);
    }
}