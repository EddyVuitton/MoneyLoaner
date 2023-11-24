using System.Net;

namespace DebtWeb.WebAPI.Data;

public class HttpApiResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public string? Message { get; set; }
    public bool IsSucces { get; set; }
}