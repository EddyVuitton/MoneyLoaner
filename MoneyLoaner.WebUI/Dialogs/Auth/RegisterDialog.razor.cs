using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MoneyLoaner.Domain.DTOs;
using MoneyLoaner.Domain.Forms;
using MoneyLoaner.WebUI.Helpers.Snackbar;
using MoneyLoaner.WebUI.Services.ApplicationService;
using MudBlazor;

namespace MoneyLoaner.WebUI.Dialogs.Auth;

public partial class RegisterDialog
{
    [Inject] public IApplicationService ApplicationService { get; set; } = null!;
    [Inject] public ISnackbarHelper SnackbarHelper { get; set; } = null!;
    [Inject] public IDialogService DialogService { get; set; } = null!;

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;

    [Parameter] public ProposalDto? Proposal { get; set; }

    private readonly RegisterAccountForm _model = new();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (Proposal is not null)
        {
            _model.Name = Proposal.Name;
            _model.Surname = Proposal.Surname;
            _model.Email = Proposal.Email;
            _model.PersonalNumber = Proposal.PersonalNumber;
        }
    }

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

            SnackbarHelper.Show("Konto zostało poprawnie zarejestrowane", Severity.Success, true, false);
            await OpenLoginDialog();
        }
        catch (Exception ex)
        {
            SnackbarHelper.Show("Konto nie zostało zarejestrowane", Severity.Warning, true, false);
            SnackbarHelper.Show(ex.Message, Severity.Error);
        }
    }

    private async Task OpenLoginDialog()
    {
        this.Cancel();
        await Task.Delay(400);

        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            NoHeader = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        };

        DialogService.Show<LoginDialog>(string.Empty, options);
    }

    private void Cancel() => MudDialog.Cancel();
}