using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UserManagement.Domain.Common.Enums;

namespace UserManagement.Infrastructure.Filters;

public class CustomValidationFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values.Where(v => v.Errors.Count > 0)
                .SelectMany(v => v.Errors)
                .Select(v => v.ErrorMessage);

            var response = new
            {
                Code = StatusCodes.WrongRequest,
                Message = string.Join(" | ", errors),
            };

            context.Result = new BadRequestObjectResult(response);
        }
    }
}