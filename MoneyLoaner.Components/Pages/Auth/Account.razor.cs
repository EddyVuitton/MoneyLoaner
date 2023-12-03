using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MoneyLoaner.ComponentsShared.Helpers;
using MoneyLoaner.ComponentsShared.Helpers.Snackbar;
using MoneyLoaner.Data.DTOs;
using MoneyLoaner.WebAPI.Auth;
using MoneyLoaner.WebAPI.Services.ApplicationService;

namespace MoneyLoaner.Components.Pages.Auth;

public partial class Account
{
#nullable disable
    [Inject] public IApplicationService ApplicationService { get; set; }
    [Inject] public ISnackbarHelper SnackbarHelper { get; set; }
    [Inject] public IJSRuntime JS { get; set; }
    [Inject] public ILoginService LoginService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
#nullable enable

    private List<LoanInstallmentDto> _installmentDtos = new();
    private AccountInfoDto? _accountInfoDto;

    protected async override Task OnInitializedAsync()
    {
        var accountId = await this.NavIfNotLoggedIn();

        await LoadData(accountId);
    }

    #region PrivateMethods

    private async Task<int> NavIfNotLoggedIn()
    {
        var isLoggedIn = await LoginService.IsLoggedInAsync();

        if (int.IsNegative(isLoggedIn))
        {
            NavigationManager.NavigateTo("/login");
        }

        return isLoggedIn;
    }

    private async Task LoadData(int accountId)
    {
        var accountInfoResult = await ApplicationService.GetAccountInfoAsync(accountId);

        if (accountInfoResult is not null && accountInfoResult.Data is not null)
        {
            accountInfoResult.Data.Phone = ComponentsHelper.FormatPhoneNumber(accountInfoResult.Data.Phone);

            if (accountInfoResult.Data.LoanId > 0)
            {
                var scheduleResult = await ApplicationService.GetScheduleAsync(accountInfoResult.Data.LoanId);
                _installmentDtos = scheduleResult.Data!;
            }
        }

        _accountInfoDto = accountInfoResult?.Data;
    }

    #endregion PrivateMethods
}