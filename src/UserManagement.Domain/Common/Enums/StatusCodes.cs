namespace UserManagement.Domain.Common.Enums;

public enum StatusCodes
{
    Success = 200,
    BadRequest = 400,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    WrongRequest = 406,
    Conflict = 409,
    GenericError = 500
}