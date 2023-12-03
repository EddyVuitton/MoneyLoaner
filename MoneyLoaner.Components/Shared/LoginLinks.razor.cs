using Microsoft.AspNetCore.Components;
using MoneyLoaner.WebAPI.Auth;

namespace MoneyLoaner.Components.Shared;

public partial class LoginLinks
{
#nullable disable
    [Inject] public ILoginService LoginService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
#nullable enable

    private void NavigateToLoginPage()
    {
        NavigationManager.NavigateTo("/login/");
    }

    private void NavigateToRegistrationPage()
    {
        NavigationManager.NavigateTo("/register/");
    }

    private async Task LogOut()
    {
        await LoginService.LogoutAsync();
        NavigationManager.NavigateTo("/");
    }
}