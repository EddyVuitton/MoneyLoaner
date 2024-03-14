using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MoneyLoaner.Domain.DTOs;
using MoneyLoaner.WebUI.Auth;
using MoneyLoaner.WebUI.Helpers;
using MoneyLoaner.WebUI.Helpers.Snackbar;
using MoneyLoaner.WebUI.Services.ApplicationService;
using MudBlazor;

namespace MoneyLoaner.WebUI.Pages;

public partial class Account
{
#nullable disable
    [Inject] public IApplicationService ApplicationService { get; set; }
    [Inject] public ISnackbarHelper SnackbarHelper { get; set; }
    [Inject] public IJSRuntime JS { get; set; }
    [Inject] public ILoginService LoginService { get; set; }
    [Inject] public IDialogService DialogService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
#nullable enable

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
        var accountInfoResult = await ApplicationService.GetAccountInfoAsync(clientId);

        if (accountInfoResult is not null && accountInfoResult.Data is not null)
        {
            accountInfoResult.Data.Phone = ComponentsHelper.FormatPhoneNumber(accountInfoResult.Data.Phone);
            accountInfoResult.Data.CCNumberToRepayment = ComponentsHelper.BasicNumberMaskFormatter(accountInfoResult.Data.CCNumberToRepayment!, "00 0000 0000 0000 0000 0000 0000", false);

            var scheduleResult = await ApplicationService.GetScheduleAsync(accountInfoResult.Data.LoanId);
            _installmentDtos = scheduleResult.Data;
        }

        var loansHistoryResult = await ApplicationService.GetLoansHistoryAsync(clientId);
        _loanHistoryDtos = loansHistoryResult.Data;

        _accountInfoDto = accountInfoResult?.Data;
    }
}