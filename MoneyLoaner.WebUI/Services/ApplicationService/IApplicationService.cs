using MoneyLoaner.Domain.Auth;
using MoneyLoaner.Domain.DTOs;
using MoneyLoaner.Domain.Forms;
using MoneyLoaner.Domain.Http;

namespace MoneyLoaner.WebUI.Services.ApplicationService;

public interface IApplicationService
{
    Task<HttpResult> SubmitNewProposalAsync(NewProposalDto newProposalDto);
    Task<HttpResultT<UserToken>> LoginAsync(LoginAccountForm loginForm);
    Task<HttpResult> RegisterAsync(RegisterAccountForm registerForm);
    Task<HttpResultT<UserAccountDto?>> GetUserAccountAsync(string email);
    Task<HttpResultT<List<LoanInstallmentDto>?>> GetScheduleAsync(int po_id);
    Task<HttpResultT<AccountInfoDto?>> GetAccountInfoAsync(int pk_id);
    Task<HttpResult> UpdateEmailAsync(int pk_id, string email);
    Task<HttpResult> UpdatePhoneAsync(int pk_id, string phone);
    Task<HttpResult> UpdatePasswordAsync(UpdatePasswordForm updatePasswordForm);
    Task<HttpResultT<List<LoanHistoryDto>?>> GetLoansHistoryAsync(int pk_id);
    Task<HttpResultT<LoanConfig?>> GetLoanConfigAsync();
}