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

    #region Account

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

    public async Task<HttpApiResponse> UpdateEmailAsync(int pk_id, string email)
    {
        var response = await _httpClient.PostAsync($"{_ACCOUNTAPI}/UpdateEmailAsync?pk_id={pk_id}&email={email}", null);
        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpApiResponse>(responseContent);

        if (deserialisedResponse is null)
            throw new NullReferenceException(typeof(HttpApiResponse).Name);

        return deserialisedResponse;
    }

    public async Task<HttpApiResponse> UpdatePhoneAsync(int pk_id, string phone)
    {
        var response = await _httpClient.PostAsync($"{_ACCOUNTAPI}/UpdatePhoneAsync?pk_id={pk_id}&phone={phone}", null);
        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpApiResponse>(responseContent);

        if (deserialisedResponse is null)
            throw new NullReferenceException(typeof(HttpApiResponse).Name);

        return deserialisedResponse;
    }

    public async Task<HttpApiResponse> UpdatePasswordAsync(UpdatePasswordForm updatePasswordForm)
    {
        var json = JsonConvert.SerializeObject(updatePasswordForm);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_ACCOUNTAPI}/UpdatePasswordAsync", content);

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpApiResponse>(responseContent);

        if (deserialisedResponse is null)
            throw new NullReferenceException(typeof(HttpApiResponse).Name);

        return deserialisedResponse;
    }

    #endregion Account

    #region Loan

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

    public async Task<HttpApiResponseT<List<LoanInstallmentDto>?>> GetScheduleAsync(int po_id)
    {
        var response = await _httpClient.GetAsync($"{_LOANAPI}/GetScheduleAsync?po_id={po_id}");

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpApiResponseT<List<LoanInstallmentDto>?>>(responseContent);

        if (deserialisedResponse is null)
            throw new NullReferenceException(typeof(List<LoanInstallmentDto>).Name);

        return deserialisedResponse;
    }

    public async Task<HttpApiResponseT<AccountInfoDto?>> GetAccountInfoAsync(int pk_id)
    {
        var response = await _httpClient.GetAsync($"{_LOANAPI}/GetAccountInfoAsync?pk_id={pk_id}");

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpApiResponseT<AccountInfoDto?>>(responseContent);

        if (deserialisedResponse is null)
            throw new NullReferenceException(typeof(AccountInfoDto).Name);

        return deserialisedResponse;
    }

    public async Task<HttpApiResponseT<List<LoanHistoryDto>?>> GetLoansHistoryAsync(int pk_id)
    {
        var response = await _httpClient.GetAsync($"{_LOANAPI}/GetLoansHistoryAsync?pk_id={pk_id}");

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpApiResponseT<List<LoanHistoryDto>?>>(responseContent);

        if (deserialisedResponse is null)
            throw new NullReferenceException(typeof(List<LoanHistoryDto>).Name);

        return deserialisedResponse;
    }

    #endregion Loan
}