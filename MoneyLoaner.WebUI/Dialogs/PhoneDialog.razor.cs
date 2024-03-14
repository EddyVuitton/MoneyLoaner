﻿using Microsoft.AspNetCore.Components;
using MoneyLoaner.Domain.DTOs;
using MoneyLoaner.Domain.FluentValidator;
using MoneyLoaner.WebUI.Helpers;
using MoneyLoaner.WebUI.Sections;
using MoneyLoaner.WebUI.Services.ApplicationService;
using MudBlazor;

namespace MoneyLoaner.WebUI.Dialogs;

public partial class PhoneDialog
{
#nullable disable
    [Inject] public IApplicationService ApplicationService { get; set; }

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
    [Parameter] public AccountInfo AccountInfoRef { get; set; }
#nullable enable

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

            AccountInfoRef.AfterChangePhoneSubmit(result.IsSucces, ComponentsHelper.FormatPhoneNumber(_proposalDto.PhoneNumber!));
        }
    }

    private void Close()
    {
        MudDialog.Cancel();
    }
}