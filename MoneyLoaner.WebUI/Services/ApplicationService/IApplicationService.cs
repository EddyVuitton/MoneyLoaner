﻿using MoneyLoaner.Domain.Auth;
using MoneyLoaner.Domain.DTOs;
using MoneyLoaner.Domain.Forms;
using MoneyLoaner.Domain.Http;

namespace MoneyLoaner.WebUI.Services.ApplicationService;

public interface IApplicationService
{
    Task<HttpApiResponse> SubmitNewProposalAsync(NewProposalDto newProposalDto);
    Task<HttpApiResponseT<UserToken>> LoginAsync(LoginAccountForm loginForm);
    Task<HttpApiResponse> RegisterAsync(RegisterAccountForm registerForm);
    Task<HttpApiResponseT<UserAccountDto?>> GetUserAccountAsync(string email);
    Task<HttpApiResponseT<List<LoanInstallmentDto>?>> GetScheduleAsync(int po_id);
    Task<HttpApiResponseT<AccountInfoDto?>> GetAccountInfoAsync(int pk_id);
    Task<HttpApiResponse> UpdateEmailAsync(int pk_id, string email);
    Task<HttpApiResponse> UpdatePhoneAsync(int pk_id, string phone);
    Task<HttpApiResponse> UpdatePasswordAsync(UpdatePasswordForm updatePasswordForm);
    Task<HttpApiResponseT<List<LoanHistoryDto>?>> GetLoansHistoryAsync(int pk_id);
    Task<HttpApiResponseT<LoanConfig?>> GetLoanConfigAsync();
}