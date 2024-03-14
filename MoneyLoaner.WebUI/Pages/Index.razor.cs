using Microsoft.AspNetCore.Components;
using MoneyLoaner.WebUI.Auth;
using MoneyLoaner.WebUI.Helpers.Snackbar;
using MoneyLoaner.WebUI.Services.ApplicationService;

namespace MoneyLoaner.WebUI.Pages;

public partial class Index
{
    [Inject] public IApplicationService ApplicationService { get; set; } = null!;
    [Inject] public ISnackbarHelper SnackbarHelper { get; set; } = null!;
    [Inject] public ILoginService LoginService { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        await LoginService.LogoutIfExpiredTokenAsync();
    }
}