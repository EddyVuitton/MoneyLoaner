using Microsoft.AspNetCore.Components;
using MoneyLoaner.Domain.DTOs;
using MoneyLoaner.Domain.FluentValidator;
using MoneyLoaner.WebUI.Sections;
using MoneyLoaner.WebUI.Services.ApplicationService;
using MudBlazor;

namespace MoneyLoaner.WebUI.Dialogs;

public partial class EmailDialog
{
    [Inject] public IApplicationService ApplicationService { get; set; } = null!;

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;
    
    [Parameter] public AccountInfo? AccountInfoRef { get; set; }

    private MudForm _form = new();
    private readonly ProposalDto _proposalDto = new();
    private readonly ProposalModelFluentValidator _proposalValidator = new();

    private async Task Submit()
    {
        if (AccountInfoRef is null || AccountInfoRef.AccountInfoDto is null)
        {
            return;
        }

        await _form.Validate();

        try
        {
            if (_form.IsValid)
            {
                await ApplicationService.UpdateEmailAsync(AccountInfoRef.AccountInfoDto.AccountId, _proposalDto.Email!);

                Close();
                AccountInfoRef.AfterChangeEmailSubmit( _proposalDto.Email!);
            }
        }
        catch (Exception ex)
        {
            AccountInfoRef.FailureAfterSubmitSnackbar(ex.Message);
        }
    }

    private void Close()
    {
        MudDialog.Cancel();
    }
}