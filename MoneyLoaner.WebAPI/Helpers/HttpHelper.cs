using DebtWeb.WebAPI.Data;

namespace MoneyLoaner.WebAPI.Helpers;

public class HttpHelper
{
    public static HttpResponse<T> Error<T>(Exception e)
    {
        var httpResponse = new HttpResponse<T>()
        {
            Data = default,
            StatusCode = System.Net.HttpStatusCode.InternalServerError,
            Message = e.Message
        };

        return httpResponse;
    }

    public static HttpResponse<T> Ok<T>(T data)
    {
        var httpResponse = new HttpResponse<T>()
        {
            Data = data,
            StatusCode = System.Net.HttpStatusCode.OK,
            Message = string.Empty
        };

        return httpResponse;
    }

    public static HttpResponse<T> NullObject<T>()
    {
        var httpResponse = new HttpResponse<T>()
        {
            Data = default,
            StatusCode = System.Net.HttpStatusCode.OK,
            Message = string.Empty
        };

        return httpResponse;
    }
}