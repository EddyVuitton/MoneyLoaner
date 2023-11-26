using DebtWeb.WebAPI.Data;
using MoneyLoaner.Data.DTOs;
using MoneyLoaner.Data.Forms;
using MoneyLoaner.WebAPI.Data;
using Newtonsoft.Json;
using System.Text;

namespace MoneyLoaner.WebAPI.Services.ApplicationService;

public class ApplicationService : IApplicationService
{
    private readonly HttpClient _httpClient;
    private const string _LOANAPI = "api/Loan";
    private const string _ACCOUNTAPI = "api/Account";

    public ApplicationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpApiResponse> SubmitNewProposalAsync(NewProposalDto newProposalDto)
    {
        var json = JsonConvert.SerializeObject(newProposalDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_LOANAPI}/SubmitNewProposalAsync", content);

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpApiResponse>(responseContent);

        if (deserialisedResponse is null)
            throw new NullReferenceException(typeof(HttpApiResponse).Name);

        return deserialisedResponse;
    }

    public async Task<HttpApiResponseT<UserToken>> LoginAsync(LoginAccountForm loginForm)
    {
        var json = JsonConvert.SerializeObject(loginForm);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_ACCOUNTAPI}/Login", content);

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpApiResponseT<UserToken>>(responseContent);

        if (deserialisedResponse is null)
            throw new NullReferenceException(typeof(UserToken).Name);

        return deserialisedResponse;
    }

    public async Task<HttpApiResponse> RegisterAsync(RegisterAccountForm registerForm)
    {
        var json = JsonConvert.SerializeObject(registerForm);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_ACCOUNTAPI}/Register", content);

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpApiResponse>(responseContent);

        if (deserialisedResponse is null)
            throw new NullReferenceException(typeof(HttpApiResponse).Name);

        return deserialisedResponse;
    }

    public async Task<HttpApiResponseT<UserAccountDto?>> GetUserAccountAsync(string email)
    {
        var response = await _httpClient.GetAsync($"{_ACCOUNTAPI}/GetUserAccount?email={email}");

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpApiResponseT<UserAccountDto?>>(responseContent);

        if (deserialisedResponse is null)
            throw new NullReferenceException(typeof(UserAccountDto).Name);

        return deserialisedResponse;
    }
}