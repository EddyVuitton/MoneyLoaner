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
        await _form.Validate();

        if (_form.IsValid)
        {
            var result = await ApplicationService.UpdateEmailAsync(1, _proposalDto.Email!);

            if (!result.IsSucces)
            {
                AccountInfoRef?.FailureAfterSubmitSnackbar(result.Message!);
                return;
            }

            Close();
            AccountInfoRef?.AfterChangeEmailSubmit(result.IsSucces, _proposalDto.Email!);
        }
    }

    private void Close()
    {
        MudDialog.Cancel();
    }
}