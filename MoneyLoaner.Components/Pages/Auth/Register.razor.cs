using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MoneyLoaner.ComponentsShared.Helpers.Snackbar;
using MoneyLoaner.Data.DTOs;
using MoneyLoaner.Data.Forms;
using MoneyLoaner.WebAPI.Extensions;
using MoneyLoaner.WebAPI.Helpers;
using MoneyLoaner.WebAPI.Services.ApplicationService;
using MudBlazor;
using System.Text.Json;

namespace MoneyLoaner.Components.Pages.Auth;

public partial class Register
{
    [Inject] public IApplicationService? ApplicationService { get; set; }
    [Inject] public ISnackbarHelper? SnackbarHelper { get; set; }
    [Inject] public IJSRuntime? JS { get; set; }
    [Inject] public NavigationManager? NavigationManager { get; set; }

    private NewProposalDto _newProposalDto = new();
    private RegisterAccountForm _model = new();

    protected override async Task OnInitializedAsync()
    {
        if (JS is not null)
        {
            var encryptedJson = await JS.GetFromLocalStorage(EncryptHelper.Encrypt("newproposal"));
            var decryptedJson = EncryptHelper.Decrypt(encryptedJson.ToString());
            var data = JsonSerializer.Deserialize<NewProposalDto>(decryptedJson);

            if (data is not null)
            {
                _newProposalDto = data;
                _model.PersonalNumber = _newProposalDto.ProposalDto!.PersonalNumber;
                _model.Email = _newProposalDto.ProposalDto!.Email;
            }
        }
    }

    private async void OnValidSubmit(EditContext context)
    {
        try
        {
            var response = await ApplicationService!.RegisterAsync((RegisterAccountForm)context.Model);

            if (!response.IsSucces)
            {
                SnackbarHelper!.Show(response.Message!, Severity.Error, true, false);
                return;
            }

            NavigationManager!.NavigateTo("login");
        }
        catch (Exception ex)
        {
            SnackbarHelper!.Show("Konto nie zostało zarejestrowane, zgłoś problem konsultantowi", Severity.Warning, true, false);
            SnackbarHelper!.Show(ex.Message, Severity.Error);
        }

        StateHasChanged();
    }
}