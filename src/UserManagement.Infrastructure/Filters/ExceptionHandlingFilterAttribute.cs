using Microsoft.AspNetCore.Mvc.Filters;
using UserManagement.Domain.Common.Enums;
using UserManagement.Domain.Common.Models;
using UserManagement.Domain.Exceptions;
using UserManagement.Infrastructure.Helpers;
using UserManagement.Infrastructure.Logger;

namespace UserManagement.Infrastructure.Filters;

public class ExceptionHandlingFilterAttribute(ILoggerService logger)
    : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
        logger.LogError(exception, $"Message: {exception.Message}");
        var (statusCode, message) = exception switch
        {
            EntityNotFoundException or KeyNotFoundException => (StatusCodes.NotFound, exception.Message),
            UserIsAlreadyAuthenticatedException => (StatusCodes.Conflict, exception.Message),
            UnauthorizedAccessException => (StatusCodes.Unauthorized, exception.Message),
            ArgumentException or InvalidOperationException or InvalidDataException or FormatException
                or OverflowException => (StatusCodes.BadRequest, exception.Message),
            _ => (StatusCodes.GenericError, "An unknown error has occurred, please contact the system administrator.")
        };
        
        var response = Result.Failed(message, statusCode);

        context.Result = ResponseHelper.CreateResponse(response);
        context.ExceptionHandled = true;
    }
}