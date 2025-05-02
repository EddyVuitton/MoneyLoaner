using MoneyLoaner.Domain.Auth;
using MoneyLoaner.Domain.DTOs;
using MoneyLoaner.Domain.Forms;
namespace MoneyLoaner.WebUI.Services.ApplicationService;

public interface IApplicationService
{
    Task SubmitNewProposalAsync(NewProposalDto newProposalDto);
    Task<UserToken?> LoginAsync(LoginAccountForm loginForm);
    Task RegisterAsync(RegisterAccountForm registerForm);
    Task<UserAccountDto?> GetUserAccountAsync(string email);
    Task<List<LoanInstallmentDto>?> GetScheduleAsync(int po_id);
    Task<AccountInfoDto?> GetAccountInfoAsync(int pk_id);
    Task UpdateEmailAsync(int pk_id, string email);
    Task UpdatePhoneAsync(int pk_id, string phone);
    Task UpdatePasswordAsync(UpdatePasswordForm updatePasswordForm);
    Task<List<LoanHistoryDto>?> GetLoansHistoryAsync(int pk_id);
    Task<LoanConfig?> GetLoanConfigAsync();
}