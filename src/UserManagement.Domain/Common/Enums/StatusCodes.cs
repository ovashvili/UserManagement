namespace UserManagement.Domain.Common.Enums;

public enum StatusCodes
{
    Success = 200,
    Created = 201,
    Accepted = 202,
    BadRequest = 400,
    Unauthorized = 401,
    NotFound = 404,
    WrongRequest = 406,
    Conflict = 409,
    GenericError = 500
}