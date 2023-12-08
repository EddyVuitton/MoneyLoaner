using Microsoft.AspNetCore.Components;
using MoneyLoaner.ComponentsShared.Dialogs.Auth;
using MoneyLoaner.WebAPI.Auth;
using MudBlazor;

namespace MoneyLoaner.Components.Shared;

public partial class LoginLinks
{
#nullable disable
    [Inject] public ILoginService LoginService { get; set; }
    [Inject] public IDialogService DialogService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
#nullable enable

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