using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MoneyLoaner.ComponentsShared.Helpers;
using MoneyLoaner.ComponentsShared.Helpers.Snackbar;
using MoneyLoaner.Data.DTOs;
using MoneyLoaner.WebAPI.Services.ApplicationService;

namespace MoneyLoaner.Components.Pages.Auth;

public partial class Account
{
#nullable disable
    [Inject] public IApplicationService ApplicationService { get; set; }
    [Inject] public ISnackbarHelper SnackbarHelper { get; set; }
    [Inject] public IJSRuntime JS { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
#nullable enable

    private List<LoanInstallmentDto> _installmentDtos = new();
    private AccountInfoDto? _accountInfoDto;

    protected async override Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        var scheduleResult = await ApplicationService.GetScheduleAsync(1);
        _installmentDtos = scheduleResult.Data!;

        var accountInfoResult = await ApplicationService.GetAccountInfoAsync(1);

        if (accountInfoResult is not null && accountInfoResult.Data is not null)
        {
            accountInfoResult.Data.Phone = ComponentsHelper.FormatPhoneNumber(accountInfoResult.Data.Phone);
        }

        _accountInfoDto = accountInfoResult?.Data;
    }
}