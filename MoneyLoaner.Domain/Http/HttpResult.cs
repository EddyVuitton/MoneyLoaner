using System.Net;

namespace MoneyLoaner.Domain.Http;

public class HttpResult
{
    public HttpStatusCode StatusCode { get; set; }
    public string? Message { get; set; }
    public bool IsSucces { get; set; }
}