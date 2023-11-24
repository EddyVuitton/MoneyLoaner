using System.Net;

namespace DebtWeb.WebAPI.Data;

public class HttpApiResponseT<T>
{
    public HttpStatusCode StatusCode { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public bool IsSuccess { get; set; }
}