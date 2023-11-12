using System.Net;

namespace DebtWeb.WebAPI.Data;

public class HttpResponse<T>
{
    public HttpStatusCode StatusCode { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
}