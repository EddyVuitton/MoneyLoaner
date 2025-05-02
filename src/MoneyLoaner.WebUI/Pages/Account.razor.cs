using Microsoft.AspNetCore.Components;
using MoneyLoaner.Domain.DTOs;
using MoneyLoaner.WebUI.Auth;
using MoneyLoaner.WebUI.Helpers;
using MoneyLoaner.WebUI.Services.ApplicationService;

namespace MoneyLoaner.WebUI.Pages;

public partial class Account
{
    [Inject] public IApplicationService ApplicationService { get; set; } = null!;
    [Inject] public ILoginService LoginService { get; set; } = null!;

    private List<LoanInstallmentDto>? _installmentDtos;
    private List<LoanHistoryDto>? _loanHistoryDtos;
    private AccountInfoDto? _accountInfoDto;
    private bool _isLoggedIn = false;
    private bool _firstEntry = true;

    protected async override Task OnInitializedAsync()
    {
        await LoginService.LogoutIfExpiredTokenAsync();
        var clientId = await LoginService.IsLoggedInAsync();

        if (clientId > 0)
        {
            await LoadData(clientId);
            _isLoggedIn = true;
        }

        _firstEntry = false;
    }

    private async Task LoadData(int clientId)
    {
        try
        {
            var result = await ApplicationService.GetAccountInfoAsync(clientId);

            if (result is not null)
            {
                result.Phone = ComponentsHelper.FormatPhoneNumber(result.Phone);
                result.CCNumberToRepayment = ComponentsHelper.BasicNumberMaskFormatter(result.CCNumberToRepayment!, "00 0000 0000 0000 0000 0000 0000", false);

                var scheduleResult = await ApplicationService.GetScheduleAsync(result.LoanId);
                _installmentDtos = scheduleResult;
            }

            var loansHistoryResult = await ApplicationService.GetLoansHistoryAsync(clientId);
            _loanHistoryDtos = loansHistoryResult;

            _accountInfoDto = result;
        }
        catch { }
    }
}