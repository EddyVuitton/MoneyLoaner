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
    [Inject] public IApplicationService ApplicationService { get; set; } = null!;
    [Inject] public ISnackbarHelper SnackbarHelper { get; set; } = null!;
    [Inject] public ILoginService LoginService { get; set; } = null!;
    [Inject] public IDialogService DialogService { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;

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