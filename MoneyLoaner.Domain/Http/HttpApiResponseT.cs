using System.Net;

namespace MoneyLoaner.Domain.Http;

public class HttpApiResponseT<T>
{
    public HttpStatusCode StatusCode { get; set; }
    public string? Message { get; set; }
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
}