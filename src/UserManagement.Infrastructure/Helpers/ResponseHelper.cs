using Microsoft.AspNetCore.Mvc;
using UserManagement.Domain.Common.Enums;
using UserManagement.Domain.Common.Models;

namespace UserManagement.Infrastructure.Helpers;

public static class ResponseHelper
{
    private static readonly Dictionary<StatusCodes, Func<Result, ActionResult>> StatusCodeMap =
        new()
        {
            { StatusCodes.Success, result => new OkObjectResult(result) },
            { StatusCodes.WrongRequest, result => new BadRequestObjectResult(result) },
            { StatusCodes.BadRequest, result => new BadRequestObjectResult(result) },
            { StatusCodes.Unauthorized, result => new UnauthorizedObjectResult(result) },
            { StatusCodes.Conflict, result => new ConflictObjectResult(result) },
            { StatusCodes.NotFound, result => new NotFoundObjectResult(result) },
            { StatusCodes.Forbidden, result => CreateCustomErrorResult(result, 403) },
        };
    
    public static ActionResult CreateResponse(Result result)
    {
        if (StatusCodeMap.TryGetValue(result.StatusCode, out var value))
            return value(result);

        return CreateCustomErrorResult(result);
    }

    public static ActionResult<Result<T>> CreateResponse<T>(Result<T> result)
    {
        if (StatusCodeMap.TryGetValue(result.StatusCode, out var value))
            return value(result);

        return CreateCustomErrorResult(result);
    }

    public static ObjectResult CreateCustomErrorResult(Result result, int statusCode = 500)
    {
        return new ObjectResult(new { result.StatusCode, result.Message })
        {
            StatusCode = statusCode
        };
    }
}
