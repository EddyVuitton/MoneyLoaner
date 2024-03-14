using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MoneyLoaner.WebUI.Auth;
using MoneyLoaner.WebUI.Helpers.Snackbar;
using MoneyLoaner.WebUI.Services.ApplicationService;

namespace MoneyLoaner.WebUI.Pages;

public partial class Index
{
#nullable disable
    [Inject] public IApplicationService ApplicationService { get; set; }
    [Inject] public ISnackbarHelper SnackbarHelper { get; set; }
    [Inject] public ILoginService LoginService { get; set; }
    [Inject] public IJSRuntime JS { get; set; }
#nullable enable

    protected override async Task OnInitializedAsync()
    {
        await LoginService.LogoutIfExpiredTokenAsync();
    }
}