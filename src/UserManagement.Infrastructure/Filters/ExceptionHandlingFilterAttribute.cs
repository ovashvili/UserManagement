using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using UserManagement.Domain.Common.Enums;
using UserManagement.Domain.Common.Models;
using UserManagement.Infrastructure.Helpers;

namespace UserManagement.Infrastructure.Filters;

public class ExceptionHandlingFilterAttribute(ILogger<ExceptionHandlingFilterAttribute> logger)
    : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
        if (logger.IsEnabled(LogLevel.Critical))
            logger.LogError(exception, $"Message: {exception.Message}");

        var response = Result.Failed(
            $"An unknown error has occurred, please contact the system administrator.", StatusCodes.GenericError);

        context.Result = ResponseHelper.CreateResponse(response);
        context.ExceptionHandled = true;
    }
}