using UserManagement.Application.Common.Enums;

namespace UserManagement.Application.Common.Models;

public class Result<TData>(StatusCode statusCode, string? message = null, TData? data = default)
{
    public StatusCode StatusCode { get; set; } = statusCode;
    public string? Message { get; set; } = message;
    public TData? Data { get; set; } = data;
}
