using Microsoft.AspNetCore.Components;
using MoneyLoaner.Domain.DTOs;
using MoneyLoaner.Domain.FluentValidator;
using MoneyLoaner.WebUI.Helpers;
using MoneyLoaner.WebUI.Sections;
using MoneyLoaner.WebUI.Services.ApplicationService;
using MudBlazor;

namespace MoneyLoaner.WebUI.Dialogs;

public partial class PhoneDialog
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
            var result = await ApplicationService.UpdatePhoneAsync(1, _proposalDto.PhoneNumber!);
            this.Close();

            AccountInfoRef?.AfterChangePhoneSubmit(result.IsSucces, ComponentsHelper.FormatPhoneNumber(_proposalDto.PhoneNumber!));
        }
    }

    private void Close()
    {
        MudDialog.Cancel();
    }
}