using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MoneyLoaner.Domain.Forms;
using MoneyLoaner.WebUI.Auth;
using MoneyLoaner.WebUI.Helpers.Snackbar;
using MoneyLoaner.WebUI.Services.ApplicationService;
using MudBlazor;

namespace MoneyLoaner.WebUI.Dialogs.Auth;

public partial class LoginDialog
{
#nullable disable
    [Inject] public IApplicationService ApplicationService { get; set; }
    [Inject] public ISnackbarHelper SnackbarHelper { get; set; }
    [Inject] public ILoginService LoginService { get; set; }
    [Inject] public IDialogService DialogService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
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

    private async Task OpenRegisterDialog()
    {
        this.Cancel();
        await Task.Delay(250);

        var options = new DialogOptions
        {
            CloseOnEscapeKey = false,
            NoHeader = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        };

        DialogService.Show<RegisterDialog>(string.Empty, options);
    }

    private void Cancel() => MudDialog.Cancel();
}