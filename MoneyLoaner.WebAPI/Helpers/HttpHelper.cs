using DebtWeb.WebAPI.Data;
using System.Net;

namespace MoneyLoaner.WebAPI.Helpers;

public class HttpHelper
{
    public static HttpApiResponseT<T> Error<T>(Exception e)
    {
        var httpResponse = new HttpApiResponseT<T>()
        {
            Data = default,
            StatusCode = HttpStatusCode.InternalServerError,
            Message = e.Message,
            IsSuccess = false
        };

        return httpResponse;
    }

    public static HttpApiResponseT<T> Ok<T>(T data, string message = "")
    {
        var httpResponse = new HttpApiResponseT<T>()
        {
            Data = data,
            StatusCode = HttpStatusCode.OK,
            Message = string.IsNullOrEmpty(message) ? "Success" : message,
            IsSuccess = true
        };

        return httpResponse;
    }

    public static HttpApiResponseT<T> Null<T>()
    {
        var httpResponse = new HttpApiResponseT<T>()
        {
            StatusCode = HttpStatusCode.InternalServerError,
            Message = string.Empty,
            Data = default,
            IsSuccess = false
        };

        return httpResponse;
    }

    public static HttpApiResponse Error(Exception ex)
    {
        var httpResponse = new HttpApiResponse()
        {
            StatusCode = HttpStatusCode.InternalServerError,
            Message = ex.Message,
            IsSucces = false
        };

        return httpResponse;
    }

    public static HttpApiResponse Ok(string message = "")
    {
        var httpResponse = new HttpApiResponse()
        {
            StatusCode = HttpStatusCode.OK,
            Message = string.IsNullOrEmpty(message) ? "Success" : message,
            IsSucces = true
        };

        return httpResponse;
    }

    public static HttpApiResponse Null()
    {
        var httpResponse = new HttpApiResponse()
        {
            StatusCode = HttpStatusCode.InternalServerError,
            Message = string.Empty,
            IsSucces = false
        };

        return httpResponse;
    }
}