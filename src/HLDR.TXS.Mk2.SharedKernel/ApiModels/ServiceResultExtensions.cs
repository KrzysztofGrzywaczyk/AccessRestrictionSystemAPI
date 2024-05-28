
using Microsoft.AspNetCore.Http;

namespace AccessControlSystem.SharedKernel.ApiModels;

public static class ServiceResultExtensions
{
    public static TResult Success<TResult, TData>(this TData result, int statusCode = StatusCodes.Status200OK)
        where TResult : ServiceResultBase<TData>, new()
        where TData : class
    {
        return new TResult
        {
            Data = result,
            StatusCode = statusCode
        };
    }

    public static TResult Failure<TResult>(this ServiceError error)
        where TResult : ServiceResultBase, new()
    {
        return new TResult
        {
            StatusCode = error.Status,
            Error = error
        };
    }
}