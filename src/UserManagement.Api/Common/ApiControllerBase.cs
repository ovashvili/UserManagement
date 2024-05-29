using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Domain.Common.Models;

namespace UserManagement.Api.Common;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(typeof(Result),StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(Result),StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(Result),StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(Result),StatusCodes.Status409Conflict)]
[ProducesResponseType(typeof(Result),StatusCodes.Status500InternalServerError)]
[ApiConventionType(typeof(DefaultApiConventions))]
public class ApiControllerBase(IMediator mediator) : ControllerBase
{
    protected readonly IMediator Mediator = mediator;
}