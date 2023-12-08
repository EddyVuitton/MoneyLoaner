using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MoneyLoaner.ComponentsShared.Helpers;
using MoneyLoaner.ComponentsShared.Helpers.Snackbar;
using MoneyLoaner.Data.DTOs;
using MoneyLoaner.WebAPI.Auth;
using MoneyLoaner.WebAPI.Services.ApplicationService;
using MudBlazor;

namespace MoneyLoaner.Components.Pages;

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
    private AccountInfoDto? _accountInfoDto;
    private bool _isLoggedIn = false;
    private bool _firstEntry = true;

    protected override async Task OnInitializedAsync()
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
            accountInfoResult.Data.CCNumberToRepayment = ComponentsHelper.BasicNumberMaskFormatter(accountInfoResult.Data.CCNumberToRepayment!, "00 0000 0000 0000 0000 0000 0000");

            if (accountInfoResult.Data.LoanId > 0)
            {
                var scheduleResult = await ApplicationService.GetScheduleAsync(accountInfoResult.Data.LoanId);
                _installmentDtos = scheduleResult.Data!;
            }
        }

        _accountInfoDto = accountInfoResult?.Data;
    }
}