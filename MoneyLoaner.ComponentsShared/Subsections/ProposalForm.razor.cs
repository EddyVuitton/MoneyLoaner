﻿using Microsoft.AspNetCore.Components;
using MoneyLoaner.ComponentsShared.Helpers.Snackbar;
using MoneyLoaner.ComponentsShared.Sections;
using MoneyLoaner.Data.DTOs;
using MoneyLoaner.Data.Forms;
using MoneyLoaner.WebAPI.Services.ApplicationService;
using MudBlazor;

namespace MoneyLoaner.ComponentsShared.Subsections;

public partial class ProposalForm
{
#nullable disable
    [Inject] public IApplicationService ApplicationService { get; set; }
    [Inject] public ISnackbarHelper SnackbarHelper { get; set; }

    [Parameter] public LoanInfo LoanInfoRef { get; set; }

    private ApplicationForm _applicationForm = new();
#nullable enable

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

    private async Task OnValidSubmit()
    {
        var proposalDto = new ProposalDto
        {
            Name = _applicationForm.Name,
            Surname = _applicationForm.Surname,
            PhoneNumber = _applicationForm.PhoneNumber,
            Email = _applicationForm.Email,
            PersonalNumber = _applicationForm.PersonalNumber,
            MonthlyIncome = _applicationForm.MonthlyIncome,
            MonthlyExpenses = _applicationForm.MonthlyExpenses,
            CCNumber = _applicationForm.CCNumber
        };

        SnackbarHelper.Show("Wniosek został wysłany", Severity.Info, true, false);
        await LoanInfoRef.SubmitNewProposal(proposalDto);
    }

    private void OnInvalidSubmit()
    {
        SnackbarHelper.Show("Uzupełnij poprawnie wszystkie pola", Severity.Warning, true, false);
    }

    private void CorrectLoan()
    {
        this.Toggle();
        LoanInfoRef.ToggleLoan();
    }

    #endregion PrivateMethods

    #region PublicMethods

    public void Toggle()
    {
        _disabled = !_disabled;
        _proposalSectionWrapperBorderStyle = _disabled ? _DISABLEBORDERSTYLE : _ACTIVEBORDERSTYLE;

        StateHasChanged();
    }

    #endregion PublicMethods
}