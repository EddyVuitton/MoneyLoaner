using DebtWeb.WebAPI.Data;
using MoneyLoaner.Data.DTOs;
using MoneyLoaner.Data.Forms;
using MoneyLoaner.WebAPI.Data;

namespace MoneyLoaner.WebAPI.Services.ApplicationService;

public interface IApplicationService
{
    Task<HttpApiResponse> SubmitNewProposalAsync(NewProposalDto newProposalDto);

    Task<HttpApiResponseT<UserToken>> LoginAsync(LoginAccountForm loginForm);

    Task<HttpApiResponse> RegisterAsync(RegisterAccountForm registerForm);

    Task<HttpApiResponseT<UserAccountDto?>> GetUserAccountAsync(string email);
}