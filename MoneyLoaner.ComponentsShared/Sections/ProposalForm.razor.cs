using Microsoft.AspNetCore.Components;
using MoneyLoaner.ComponentsShared.Helpers.Snackbar;
using MoneyLoaner.Data.DTOs;
using MoneyLoaner.Data.FluentValidator;
using MoneyLoaner.WebAPI.Services.ApplicationService;
using MudBlazor;

namespace MoneyLoaner.ComponentsShared.Sections;

public partial class ProposalForm
{
    [Inject] public IApplicationService? ApplicationService { get; set; }
    [Inject] public ISnackbarHelper? SnackbarHelper { get; set; }

    [Parameter] public Action? ParamCorrectLoan { get; set; }
    [Parameter] public EventCallback<ProposalDto> ParamSubmitNewProposal { get; set; }

    private MudForm? _form;
    private ProposalDto? _proposalDto;
    private ProposalModelFluentValidator? _proposalValidator;

    private bool _disabled = true;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _proposalDto = new()
        {
            Name = "X",
            Surname = "Y",
            CCNumber = "11111111111111111111111111",
            Email = "abc@xyz.com",
            MonthlyExpenses = 100,
            MonthlyIncome = 200,
            PersonalNumber = "00221601352",
            PhoneNumber = "123456789"
        };
        _proposalValidator = new();
    }

    private async Task Submit()
    {
        if (_form is not null)
        {
            await _form.Validate();

            if (_proposalDto is not null)
            {
                await ParamSubmitNewProposal.InvokeAsync(_proposalDto);
            }
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