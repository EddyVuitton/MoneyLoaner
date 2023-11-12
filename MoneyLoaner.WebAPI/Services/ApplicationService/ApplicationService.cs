using DebtWeb.WebAPI.Data;
using MoneyLoaner.Data.DTOs;
using Newtonsoft.Json;

namespace MoneyLoaner.WebAPI.Services.ApplicationService;

public class ApplicationService : IApplicationService
{
    private readonly HttpClient _httpClient;
    private const string _URL = "api/main";

    public ApplicationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponse<TestModelDto>> GetTest()
    {
        var response = await _httpClient.GetAsync($"{_URL}/GetTest");

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResponse<TestModelDto>>(responseContent);

        if (deserialisedResponse is null)
            throw new NullReferenceException();

        return deserialisedResponse;
    }
}