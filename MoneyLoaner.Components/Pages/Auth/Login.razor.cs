using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MoneyLoaner.ComponentsShared.Helpers.Snackbar;
using MoneyLoaner.Data.Forms;
using MoneyLoaner.WebAPI.Auth;
using MoneyLoaner.WebAPI.Services.ApplicationService;
using MudBlazor;

namespace MoneyLoaner.Components.Pages.Auth;

public partial class Login
{
#nullable disable
    [Inject] public IApplicationService ApplicationService { get; set; }
    [Inject] public ISnackbarHelper SnackbarHelper { get; set; }
    [Inject] public IJSRuntime JS { get; set; }
    [Inject] public ILoginService LoginService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
#nullable enable

    private readonly LoginAccountForm _model = new();

    private async void OnValidSubmit(EditContext context)
    {
        try
        {
            var loginForm = (LoginAccountForm)context.Model;
            var response = await ApplicationService!.LoginAsync(loginForm);

            if (!response.IsSuccess)
            {
                SnackbarHelper!.Show(response.Message!, Severity.Error, true, false);
                return;
            }

            if (response.Data is not null)
            {
                await LoginService!.LoginAsync(response.Data);
                NavigationManager!.NavigateTo("account");
            }
        }
        catch (Exception ex)
        {
            SnackbarHelper!.Show(ex.Message, Severity.Error);
        }
    }
}