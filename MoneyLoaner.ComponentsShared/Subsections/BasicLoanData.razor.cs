using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MoneyLoaner.ComponentsShared.Sections;
using MoneyLoaner.Data.DTOs;
using MoneyLoaner.WebAPI.Extensions;
using MoneyLoaner.WebAPI.Helpers;
using System.Text.Json;

namespace MoneyLoaner.ComponentsShared.Subsections;

public partial class BasicLoanData
{
#nullable disable
    [Inject] public IJSRuntime JS { get; set; }

    [Parameter] public LoanDto Loan { get; set; }
    [Parameter] public LoanConfig LoanConfig { get; set; }
    [Parameter] public LoanInfo LoanInfoRef { get; set; }
#nullable enable

    //private LoanDto Loan = new();
    private List<InstallmentDto> _installmentListDto = new();

    private string _loanSectionWrapperBorderStyle = string.Empty;
    private const string _ACTIVEBORDERSTYLE = "border: 2px solid #594ae2;";
    private const string _DISABLEBORDERSTYLE = "border: 1px solid #cccccc;";

    private decimal _firstInstallmentTotal;
    private decimal _xirr;
    private DateTime _lastAPRCalculation;
    private bool _disabled = false;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        await LoadDefulatValues();
    }

    protected override async Task OnAfterRenderAsync(bool isFirst)
    {
        await base.OnAfterRenderAsync(isFirst);

        var now = DateTime.Now;
        var oneSecondPassed = (now - _lastAPRCalculation).TotalSeconds > 1;

        if (oneSecondPassed)
        {
            CalculateXIRR();
        }
    }

    #region PrivateMethods

    private async Task LoadDefulatValues()
    {
        _loanSectionWrapperBorderStyle = _ACTIVEBORDERSTYLE;
        CalculateXIRR();
        await CalculateInstallments();
    }

    private void LoanAmountPlus()
    {
        if (Loan.Principal < LoanConfig.AmountMax)
        {
            Loan.Principal += LoanConfig.AmountStep;
        }
    }

    private void LoanAmountMinus()
    {
        if (Loan.Principal > LoanConfig.AmountMin)
        {
            Loan.Principal -= LoanConfig.AmountStep;
        }
    }

    private void LoanPeriodPlus()
    {
        if (Loan.Installments < LoanConfig.PeriodMax)
        {
            Loan.Installments += Convert.ToInt32(LoanConfig.PeriodStep);
        }
    }

    private void LoanPeriodMinus()
    {
        if (Loan.Installments > LoanConfig.PeriodMin)
        {
            Loan.Installments -= Convert.ToInt32(LoanConfig.PeriodStep);
        }
    }

    private async Task LoanValueChanged(decimal value)
    {
        Loan.Principal = value;
        Loan.Fee = value * LoanConfig.Fee;
        await CalculateInstallments();
        StateHasChanged();

        LoanInfoRef.UpdateLoan(Loan);
    }

    private async Task LoanPeriodValueChanged(decimal value)
    {
        LoanConfig.Period = value;
        Loan!.Installments = Convert.ToInt32(LoanConfig.Period);
        await CalculateInstallments();
        StateHasChanged();

        LoanInfoRef.UpdateLoan(Loan);
    }

    private void ConfirmLoan()
    {
        Toggle();
        LoanInfoRef.ToggleProposal();
    }

    private void CalculateXIRR()
    {
        _xirr = LoanHelper.CalculateXIRR(Loan);
        _lastAPRCalculation = DateTime.Now;
        Loan.XIRR = _xirr;

        StateHasChanged();
    }

    #endregion PrivateMethods

    #region PublicMethods

    public async Task CalculateInstallments()
    {
        _installmentListDto = LoanHelper.GetInstallmentList(Loan!);
        _firstInstallmentTotal = _installmentListDto.First().Total;
        Loan.InstallmentDtoList = _installmentListDto;
        LoanInfoRef.UpdateLoan(Loan);

        var json = JsonSerializer.Serialize(Loan);
        await JS.SetInLocalStorage(EncryptHelper.Encrypt("loan"), EncryptHelper.Encrypt(json));
    }

    public void Toggle()
    {
        _disabled = !_disabled;
        _loanSectionWrapperBorderStyle = _disabled ? _DISABLEBORDERSTYLE : _ACTIVEBORDERSTYLE;

        StateHasChanged();
    }

    #endregion PublicMethods
}