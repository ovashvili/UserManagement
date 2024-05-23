namespace UserManagement.Application.Common.Enums;

public enum StatusCode
{
    Success = 200,
    Created,
    Accepted,
    BadRequest = 400,
    Unauthorized,
    NotFound = 404,
    Conflict = 409,
    GenericError = 500
}