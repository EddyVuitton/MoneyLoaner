using MoneyLoaner.Domain.Http;
using System.Net;

namespace MoneyLoaner.WebAPI.Helpers;

public class HttpApiHelper
{
    public static HttpResultT<T> Error<T>(Exception e)
    {
        var httpResponse = new HttpResultT<T>()
        {
            Data = default,
            StatusCode = HttpStatusCode.InternalServerError,
            Message = e.Message,
            IsSuccess = false
        };

        return httpResponse;
    }

    public static HttpResultT<T> Ok<T>(T data, string message = "")
    {
        var httpResponse = new HttpResultT<T>()
        {
            Data = data,
            StatusCode = HttpStatusCode.OK,
            Message = string.IsNullOrEmpty(message) ? "Success" : message,
            IsSuccess = true
        };

        return httpResponse;
    }

    public static HttpResult Error(Exception ex)
    {
        var httpResponse = new HttpResult()
        {
            StatusCode = HttpStatusCode.InternalServerError,
            Message = ex.Message,
            IsSucces = false
        };

        return httpResponse;
    }

    public static HttpResult Ok(string message = "")
    {
        var httpResponse = new HttpResult()
        {
            StatusCode = HttpStatusCode.OK,
            Message = string.IsNullOrEmpty(message) ? "Success" : message,
            IsSucces = true
        };

        return httpResponse;
    }
}