using Microsoft.AspNetCore.Components;
using MoneyLoaner.ComponentsShared.Helpers.Snackbar;
using MoneyLoaner.ComponentsShared.Sections;
using MoneyLoaner.Data.DTOs;
using MoneyLoaner.Data.FluentValidator;
using MoneyLoaner.WebAPI.Services.ApplicationService;
using MudBlazor;

namespace MoneyLoaner.ComponentsShared.Subsections;

public partial class ProposalForm
{
#nullable disable
    [Inject] public IApplicationService ApplicationService { get; set; }
    [Inject] public ISnackbarHelper SnackbarHelper { get; set; }

    [Parameter] public LoanInfo LoanInfoRef { get; set; }
#nullable enable

    private MudForm? _form;
    private ProposalDto _proposalDto = new();
    private readonly ProposalModelFluentValidator _proposalValidator = new();

    private string _proposalSectionWrapperBorderStyle = string.Empty;
    private const string _ACTIVEBORDERSTYLE = "border: 2px solid #594ae2;";
    private const string _DISABLEBORDERSTYLE = "border: 1px solid #cccccc;";

    private bool _disabled = true;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _proposalSectionWrapperBorderStyle = _DISABLEBORDERSTYLE;
    }

    #region PrivateMethods

    private async Task Submit()
    {
        if (_form is not null)
        {
            await _form.Validate();

            if (_form.IsValid)
            {
                await LoanInfoRef.SubmitNewProposal(_proposalDto);
            }
        }
    }

    private void CorrectLoan()
    {
        this.Toggle();
        LoanInfoRef.ToggleLoan();
    }

    public void Toggle()
    {
        _disabled = !_disabled;
        _proposalSectionWrapperBorderStyle = _disabled ? _DISABLEBORDERSTYLE : _ACTIVEBORDERSTYLE;

        StateHasChanged();
    }

    #endregion PrivateMethods
}