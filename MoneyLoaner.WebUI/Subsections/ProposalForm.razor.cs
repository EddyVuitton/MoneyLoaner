﻿using Microsoft.AspNetCore.Components;
using MoneyLoaner.Domain.DTOs;
using MoneyLoaner.Domain.Forms;
using MoneyLoaner.WebUI.Helpers.Snackbar;
using MoneyLoaner.WebUI.Sections;
using MudBlazor;

namespace MoneyLoaner.WebUI.Subsections;

public partial class ProposalForm
{
    [Inject] public ISnackbarHelper SnackbarHelper { get; set; } = null!;

    [Parameter] public LoanInfo? LoanInfoRef { get; set; }

    private readonly ApplicationForm _applicationForm = new();

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
        if (LoanInfoRef is not null)
            await LoanInfoRef.SubmitNewProposal(proposalDto);
    }

    private void OnInvalidSubmit()
    {
        SnackbarHelper.Show("Uzupełnij poprawnie wszystkie pola", Severity.Warning, true, false);
    }

    private void CorrectLoan()
    {
        Toggle();
        LoanInfoRef?.ToggleLoan();
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