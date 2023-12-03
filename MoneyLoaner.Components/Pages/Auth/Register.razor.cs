using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MoneyLoaner.ComponentsShared.Helpers.Snackbar;
using MoneyLoaner.Data.DTOs;
using MoneyLoaner.Data.Forms;
using MoneyLoaner.WebAPI.Services.ApplicationService;
using MudBlazor;

namespace MoneyLoaner.Components.Pages.Auth;

public partial class Register
{
#nullable disable
    [Inject] public IApplicationService ApplicationService { get; set; }
    [Inject] public ISnackbarHelper SnackbarHelper { get; set; }
    [Inject] public IJSRuntime JS { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
#nullable enable

    private readonly RegisterAccountForm _model = new();

    private async void OnValidSubmit(EditContext context)
    {
        try
        {
            var response = await ApplicationService.RegisterAsync((RegisterAccountForm)context.Model);

            if (!response.IsSucces)
            {
                SnackbarHelper.Show(response.Message!, Severity.Error, true, false);
                return;
            }

            NavigationManager!.NavigateTo("login");
        }
        catch (Exception ex)
        {
            SnackbarHelper.Show("Konto nie zostało zarejestrowane", Severity.Warning, true, false);
            SnackbarHelper.Show(ex.Message, Severity.Error);
        }
    }
}