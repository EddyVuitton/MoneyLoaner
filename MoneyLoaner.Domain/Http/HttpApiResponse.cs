using System.Net;

namespace MoneyLoaner.Data.Http;

public class HttpApiResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public string? Message { get; set; }
    public bool IsSucces { get; set; }
}