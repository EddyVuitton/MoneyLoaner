﻿using Microsoft.AspNetCore.Components;
using MoneyLoaner.ComponentsShared.Sections;
using MoneyLoaner.Data.DTOs;
using MoneyLoaner.Data.FluentValidator;
using MoneyLoaner.WebAPI.Services.ApplicationService;
using MudBlazor;

namespace MoneyLoaner.ComponentsShared.Dialogs;

public partial class EmailDialog
{
#nullable disable
    [Inject] public IApplicationService ApplicationService { get; set; }

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
    [Parameter] public AccountInfo AccountInfoRef { get; set; }
#nullable enable

    private MudForm _form = new();
    private readonly ProposalDto _proposalDto = new();
    private readonly ProposalModelFluentValidator _proposalValidator = new();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    private async Task Submit()
    {
        await _form.Validate();

        if (_form.IsValid)
        {
            var result = await ApplicationService.UpdateEmailAsync(1, _proposalDto.Email!);
            this.Close();

            AccountInfoRef.AfterChangeEmailSubmit(result.IsSucces, _proposalDto.Email!);
        }
    }

    private void Close()
    {
        MudDialog.Cancel();
    }
}