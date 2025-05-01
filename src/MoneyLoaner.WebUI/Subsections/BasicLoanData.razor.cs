using Microsoft.AspNetCore.Components;
using MoneyLoaner.Domain.DTOs;
using MoneyLoaner.Domain.Helpers;
using MoneyLoaner.WebUI.Sections;
using MoneyLoaner.WebUI.Services.ApplicationService;

namespace MoneyLoaner.WebUI.Subsections;

public partial class BasicLoanData
{
    [Inject] public IApplicationService ApplicationService { get; set; } = null!;

    [Parameter] public LoanInfo? LoanInfoRef { get; set; }

    private List<InstallmentDto> _installmentListDto = new();
    private LoanDto Loan = new();
    private LoanConfig LoanConfig = new();

    private string _loanSectionWrapperBorderStyle = string.Empty;
    private const string _ACTIVEBORDERSTYLE = "border: 2px solid #594ae2;";
    private const string _DISABLEBORDERSTYLE = "border: 1px solid #cccccc;";

    private decimal _firstInstallmentTotal;
    private decimal _xirr;
    private DateTime _lastAPRCalculation;
    private bool _disabled = false;
    private bool _isInitialized = false;

    private readonly DateTime _initialNow = DateTime.Now;

    protected override async Task OnInitializedAsync()
    {
        await LoadDefulatValues();
    }

    protected override async Task OnAfterRenderAsync(bool isFirst)
    {
        await base.OnAfterRenderAsync(isFirst);

        var now = DateTime.Now;
        var oneSecondPassed = (now - _lastAPRCalculation).TotalSeconds > 1;

        if (oneSecondPassed && _isInitialized)
        {
            CalculateXIRR();
        }
    }

    #region PrivateMethods

    private async Task LoadDefulatValues()
    {
        _loanSectionWrapperBorderStyle = _ACTIVEBORDERSTYLE;
        var resultLoanConfig = await ApplicationService.GetLoanConfigAsync();

        LoanConfig = resultLoanConfig.Data ?? new()
        {
            Amount = 5000,
            AmountMin = 1000,
            AmountMax = 25000,
            AmountStep = 100,
            Period = 12,
            PeriodMin = 6,
            PeriodMax = 72,
            PeriodStep = 3,
            Fee = 0.16m,
            ContractualInterest = 0.1575m
        };

        Loan = new LoanDto
        {
            StartDate = _initialNow,
            FirstInstallmentPaymentDate = _initialNow.AddMonths(1),
            DayOfDatePayment = _initialNow.Date.Day,
            Installments = Convert.ToInt32(LoanConfig.Period),
            Principal = LoanConfig.Amount,
            Fee = LoanConfig.Amount * LoanConfig.Fee,
            InterestRate = LoanConfig.ContractualInterest
        };

        CalculateXIRR();
        CalculateInstallments();
        LoanInfoRef?.UpdateLoan(Loan);

        _isInitialized = true;
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

    private void LoanValueChanged(decimal value)
    {
        Loan.Principal = value;
        Loan.Fee = value * LoanConfig.Fee;
        CalculateInstallments();
        StateHasChanged();

        LoanInfoRef?.UpdateLoan(Loan);
    }

    private void LoanPeriodValueChanged(decimal value)
    {
        LoanConfig.Period = int.Parse(value.ToString());
        Loan!.Installments = Convert.ToInt32(LoanConfig.Period);
        CalculateInstallments();
        StateHasChanged();

        LoanInfoRef?.UpdateLoan(Loan);
    }

    private void ConfirmLoan()
    {
        Toggle();
        CalculateXIRR();
        LoanInfoRef?.ToggleProposal();
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

    public void CalculateInstallments()
    {
        _installmentListDto = LoanHelper.GetInstallmentList(Loan!);
        _firstInstallmentTotal = _installmentListDto.First().Total;
        Loan.InstallmentDtoList = _installmentListDto;
        LoanInfoRef?.UpdateLoan(Loan);
    }

    public void Toggle()
    {
        _disabled = !_disabled;
        _loanSectionWrapperBorderStyle = _disabled ? _DISABLEBORDERSTYLE : _ACTIVEBORDERSTYLE;

        StateHasChanged();
    }

    #endregion PublicMethods
}