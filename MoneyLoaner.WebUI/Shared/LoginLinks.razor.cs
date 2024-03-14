using Microsoft.AspNetCore.Components;
using MoneyLoaner.WebUI.Auth;
using MoneyLoaner.WebUI.Dialogs.Auth;
using MudBlazor;

namespace MoneyLoaner.WebUI.Shared;

public partial class LoginLinks
{
    [Inject] public ILoginService LoginService { get; set; } = null!;
    [Inject] public IDialogService DialogService { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    private async Task LogOut()
    {
        await LoginService.LogoutAsync();
        NavigationManager.NavigateTo("/");
    }

    private void OpenLoginDialog()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            NoHeader = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        };

        DialogService.Show<LoginDialog>(string.Empty, options);
    }
}