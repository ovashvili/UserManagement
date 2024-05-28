using UserManagement.Domain.Common.Enums;

namespace UserManagement.Domain.Common.Models;

public class Result
{
    public bool IsSuccessStatusCode { get; set; }
    public string? Message { get; set; }
    public StatusCodes StatusCode { get; set; }

    public static Result Succeed(StatusCodes code = StatusCodes.Success)
    {
        return new Result
        {
            StatusCode = code,
            IsSuccessStatusCode = true
        };
    }

    public static Result Failed(string message, StatusCodes code = StatusCodes.GenericError)
    {
        return new Result
        { 
            StatusCode = code,
            IsSuccessStatusCode = false,
            Message = message
        };
    }
}

public class Result<TData> : Result
{
    public TData Data { get; set; }

    public static Result<TData> Succeed(TData data, StatusCodes code = StatusCodes.Success)
    {
        return new Result<TData>
        {
            StatusCode = code,
            IsSuccessStatusCode = true,
            Data = data
        };
    }
    
    public new static Result<TData> Failed(string message, StatusCodes code = StatusCodes.Success)
    {
        return new Result<TData>
        {
            StatusCode = code,
            IsSuccessStatusCode = false,
            Message = message
        };
    }
}