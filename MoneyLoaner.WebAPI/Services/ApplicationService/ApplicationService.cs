using DebtWeb.WebAPI.Data;
using MoneyLoaner.Data.DTOs;
using MoneyLoaner.WebAPI.Helpers;
using Newtonsoft.Json;
using System.Collections;
using System.Text;

namespace MoneyLoaner.WebAPI.Services.ApplicationService;

public class ApplicationService : IApplicationService
{
    private readonly HttpClient _httpClient;
    private const string _URL = "api/Loan";

    public ApplicationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponse<Hashtable>> SubmitNewProposalAsync(NewProposalDto newProposalDto)
    {
        var json = JsonConvert.SerializeObject(newProposalDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_URL}/SubmitNewProposalAsync", content);

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResponse<Hashtable>>(responseContent);

        return deserialisedResponse ?? HttpHelper.NullObject<Hashtable>();
    }
}