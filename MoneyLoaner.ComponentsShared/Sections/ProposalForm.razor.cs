using Microsoft.AspNetCore.Components;
using MoneyLoaner.Data.DTOs;
using MoneyLoaner.Data.FluentValidator;
using MudBlazor;

namespace MoneyLoaner.ComponentsShared.Sections;

public partial class ProposalForm
{
    [Parameter] public Action? ParamCorrectLoan { get; set; }

    MudForm? form;
    ProposalDto? model;
    ProposalModelFluentValidator? proposalValidator;

    private bool _disabled = true;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        model = new();
        proposalValidator = new();
    }

    private async Task Submit()
    {
        if (form is not null)
        {
            await form.Validate();

            //...
        }
    }

    public void EnableFields()
    {
        _disabled = !_disabled;
        StateHasChanged();
    }

    public void CorrectLoan()
    {
        _disabled = !_disabled;
        ParamCorrectLoan?.Invoke();
        StateHasChanged();
    }
}