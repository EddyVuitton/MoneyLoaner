using MoneyLoaner.Domain.Auth;
using MoneyLoaner.Domain.DTOs;
using MoneyLoaner.Domain.Forms;
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

    public async Task<UserToken?> LoginAsync(LoginAccountForm loginForm)
    {
        var json = JsonConvert.SerializeObject(loginForm);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_ACCOUNTAPI}/Login", content);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<UserToken>(responseContent);

        return deserialisedResponse;
    }

    public async Task RegisterAsync(RegisterAccountForm registerForm)
    {
        var json = JsonConvert.SerializeObject(registerForm);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_ACCOUNTAPI}/Register", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task<UserAccountDto?> GetUserAccountAsync(string email)
    {
        var response = await _httpClient.GetAsync($"{_ACCOUNTAPI}/GetUserAccount?email={email}");
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<UserAccountDto>(responseContent);

        return deserialisedResponse;
    }

    public async Task UpdateEmailAsync(int pk_id, string email)
    {
        var response = await _httpClient.PostAsync($"{_ACCOUNTAPI}/UpdateEmailAsync?pk_id={pk_id}&email={email}", null);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdatePhoneAsync(int pk_id, string phone)
    {
        var response = await _httpClient.PostAsync($"{_ACCOUNTAPI}/UpdatePhoneAsync?pk_id={pk_id}&phone={phone}", null);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdatePasswordAsync(UpdatePasswordForm updatePasswordForm)
    {
        var json = JsonConvert.SerializeObject(updatePasswordForm);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_ACCOUNTAPI}/UpdatePasswordAsync", content);
        response.EnsureSuccessStatusCode();
    }

    #endregion Account

    #region Loan

    public async Task SubmitNewProposalAsync(NewProposalDto newProposalDto)
    {
        var json = JsonConvert.SerializeObject(newProposalDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_LOANAPI}/SubmitNewProposalAsync", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<LoanInstallmentDto>?> GetScheduleAsync(int po_id)
    {
        var response = await _httpClient.GetAsync($"{_LOANAPI}/GetScheduleAsync?po_id={po_id}");
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<List<LoanInstallmentDto>>(responseContent);

        return deserialisedResponse;
    }

    public async Task<AccountInfoDto?> GetAccountInfoAsync(int pk_id)
    {
        var response = await _httpClient.GetAsync($"{_LOANAPI}/GetAccountInfoAsync?pk_id={pk_id}");
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<AccountInfoDto>(responseContent);

        return deserialisedResponse;
    }

    public async Task<List<LoanHistoryDto>?> GetLoansHistoryAsync(int pk_id)
    {
        var response = await _httpClient.GetAsync($"{_LOANAPI}/GetLoansHistoryAsync?pk_id={pk_id}");
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<List<LoanHistoryDto>>(responseContent);

        return deserialisedResponse;
    }

    public async Task<LoanConfig?> GetLoanConfigAsync()
    {
        var response = await _httpClient.GetAsync($"{_LOANAPI}/GetLoanConfigAsync");
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<LoanConfig>(responseContent);

        return deserialisedResponse;
    }

    #endregion Loan
}