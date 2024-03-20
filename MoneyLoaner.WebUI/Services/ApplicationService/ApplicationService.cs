using MoneyLoaner.Domain.Auth;
using MoneyLoaner.Domain.DTOs;
using MoneyLoaner.Domain.Forms;
using MoneyLoaner.Domain.Http;
using Newtonsoft.Json;
using System.Text;

namespace MoneyLoaner.WebUI.Services.ApplicationService;

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

    public async Task<HttpResultT<UserToken>> LoginAsync(LoginAccountForm loginForm)
    {
        var json = JsonConvert.SerializeObject(loginForm);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_ACCOUNTAPI}/Login", content);

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResultT<UserToken>>(responseContent);

        if (deserialisedResponse is null)
            throw new NullReferenceException(typeof(UserToken).Name);

        return deserialisedResponse;
    }

    public async Task<HttpResult> RegisterAsync(RegisterAccountForm registerForm)
    {
        var json = JsonConvert.SerializeObject(registerForm);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_ACCOUNTAPI}/Register", content);

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResult>(responseContent);

        if (deserialisedResponse is null)
            throw new NullReferenceException(typeof(HttpResult).Name);

        return deserialisedResponse;
    }

    public async Task<HttpResultT<UserAccountDto?>> GetUserAccountAsync(string email)
    {
        var response = await _httpClient.GetAsync($"{_ACCOUNTAPI}/GetUserAccount?email={email}");
        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResultT<UserAccountDto?>>(responseContent);

        if (deserialisedResponse is null)
            throw new NullReferenceException(typeof(UserAccountDto).Name);

        return deserialisedResponse;
    }

    public async Task<HttpResult> UpdateEmailAsync(int pk_id, string email)
    {
        var response = await _httpClient.PostAsync($"{_ACCOUNTAPI}/UpdateEmailAsync?pk_id={pk_id}&email={email}", null);
        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResult>(responseContent);

        if (deserialisedResponse is null)
            throw new NullReferenceException(typeof(HttpResult).Name);

        return deserialisedResponse;
    }

    public async Task<HttpResult> UpdatePhoneAsync(int pk_id, string phone)
    {
        var response = await _httpClient.PostAsync($"{_ACCOUNTAPI}/UpdatePhoneAsync?pk_id={pk_id}&phone={phone}", null);
        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResult>(responseContent);

        if (deserialisedResponse is null)
            throw new NullReferenceException(typeof(HttpResult).Name);

        return deserialisedResponse;
    }

    public async Task<HttpResult> UpdatePasswordAsync(UpdatePasswordForm updatePasswordForm)
    {
        var json = JsonConvert.SerializeObject(updatePasswordForm);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_ACCOUNTAPI}/UpdatePasswordAsync", content);

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResult>(responseContent);

        if (deserialisedResponse is null)
            throw new NullReferenceException(typeof(HttpResult).Name);

        return deserialisedResponse;
    }

    #endregion Account

    #region Loan

    public async Task<HttpResult> SubmitNewProposalAsync(NewProposalDto newProposalDto)
    {
        var json = JsonConvert.SerializeObject(newProposalDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_LOANAPI}/SubmitNewProposalAsync", content);

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResult>(responseContent);

        if (deserialisedResponse is null)
            throw new NullReferenceException(typeof(HttpResult).Name);

        return deserialisedResponse;
    }

    public async Task<HttpResultT<List<LoanInstallmentDto>?>> GetScheduleAsync(int po_id)
    {
        var response = await _httpClient.GetAsync($"{_LOANAPI}/GetScheduleAsync?po_id={po_id}");

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResultT<List<LoanInstallmentDto>?>>(responseContent);

        if (deserialisedResponse is null)
            throw new NullReferenceException(typeof(List<LoanInstallmentDto>).Name);

        return deserialisedResponse;
    }

    public async Task<HttpResultT<AccountInfoDto?>> GetAccountInfoAsync(int pk_id)
    {
        var response = await _httpClient.GetAsync($"{_LOANAPI}/GetAccountInfoAsync?pk_id={pk_id}");

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResultT<AccountInfoDto?>>(responseContent);

        if (deserialisedResponse is null)
            throw new NullReferenceException(typeof(AccountInfoDto).Name);

        return deserialisedResponse;
    }

    public async Task<HttpResultT<List<LoanHistoryDto>?>> GetLoansHistoryAsync(int pk_id)
    {
        var response = await _httpClient.GetAsync($"{_LOANAPI}/GetLoansHistoryAsync?pk_id={pk_id}");

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResultT<List<LoanHistoryDto>?>>(responseContent);

        if (deserialisedResponse is null)
            throw new NullReferenceException(typeof(List<LoanHistoryDto>).Name);

        return deserialisedResponse;
    }

    public async Task<HttpResultT<LoanConfig?>> GetLoanConfigAsync()
    {
        var response = await _httpClient.GetAsync($"{_LOANAPI}/GetLoanConfigAsync");

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<HttpResultT<LoanConfig?>>(responseContent);

        if (deserialisedResponse is null)
            throw new NullReferenceException(typeof(LoanConfig).Name);

        return deserialisedResponse;
    }

    #endregion Loan
}