using MoneyLoaner.Data.DTOs;
using MoneyLoaner.Data.FluentValidator;
using MudBlazor;

namespace MoneyLoaner.ComponentsShared.Sections;

public partial class ProposalForm
{
    MudForm? form;
    ProposalDto? model;
    ProposalModelFluentValidator? proposalValidator;

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
}