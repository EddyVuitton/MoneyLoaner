using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MoneyLoaner.ComponentsShared.Helpers.Snackbar;
using MoneyLoaner.ComponentsShared.Subsections;
using MoneyLoaner.Data.DTOs;
using MoneyLoaner.WebAPI.Services.ApplicationService;
using MudBlazor;

namespace MoneyLoaner.ComponentsShared.Sections;

public partial class LoanInfo
{
#nullable disable
    [Inject] public IApplicationService ApplicationService { get; set; }
    [Inject] public ISnackbarHelper SnackbarHelper { get; set; }
    [Inject] public IJSRuntime JS { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
#nullable enable

    private BasicLoanData _basicLoanDataRef = new();
    private ProposalForm _proposalFormRef = new();

    private NewProposalDto? _newProposalDto = new();
    private LoanDto _loan = new();
    private List<InstallmentDto> _installmentListDto = new();
    private LoanConfig _loanConfig = new();

    private readonly DateTime _initialNow = DateTime.Now;

    private bool _submitProposalLoading = false;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        LoadDefualtLoan();
    }

    #region PrivateMethods

    private void LoadDefualtLoan()
    {
        _loanConfig = new()
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
            InterestRate = 0.1575m
        };

        _loan = new LoanDto
        {
            StartDate = _initialNow,
            FirstInstallmentPaymentDate = _initialNow.AddMonths(1),
            DayOfDatePayment = _initialNow.Date.Day,
            Installments = Convert.ToInt32(_loanConfig.Period),
            Principal = _loanConfig.Amount,
            Fee = _loanConfig.Amount * _loanConfig.Fee,
            InterestRate = _loanConfig.InterestRate
        };
    }

    private async Task ChangeDatePayment(DateTime? day)
    {
        if (day is not null)
        {
            _loan.DayOfDatePayment = day.Value.Day;
            await _basicLoanDataRef.CalculateInstallments();

            StateHasChanged();
        }
    }

    #endregion PrivateMethods

    #region PublicMethods

    public async Task SubmitNewProposal(ProposalDto proposalDto)
    {
        _submitProposalLoading = true;
        StateHasChanged();

        await Task.Delay(3000);

        _newProposalDto = new() { LoanDto = _loan, ProposalDto = proposalDto };

        try
        {
            var newProposal = await ApplicationService!.SubmitNewProposalAsync(_newProposalDto);
            //await JS.RemoveItemFromLocalStorage(EncryptHelper.Encrypt("loan"));

            if (!newProposal.IsSucces)
            {
                throw new Exception(newProposal.Message!);
            }

            var customerInfo = await ApplicationService!.GetUserAccountAsync(proposalDto.Email!);

            if (customerInfo.IsSuccess && customerInfo.Data is not null)
            {
                NavigationManager?.NavigateTo("login");
            }
            else
            {
                NavigationManager?.NavigateTo("register");
            }
        }
        catch (Exception ex)
        {
            SnackbarHelper!.Show(ex.Message, Severity.Error);
            _submitProposalLoading = false;
            StateHasChanged();
        }
    }

    public void UpdateLoan(LoanDto loan)
    {
        if (loan is null || loan.InstallmentDtoList is null)
            return;

        _loan = loan;
        _installmentListDto = loan.InstallmentDtoList;
        StateHasChanged();
    }

    public void ToggleLoan()
    {
        _basicLoanDataRef.Toggle();
        StateHasChanged();
    }

    public void ToggleProposal()
    {
        _proposalFormRef.Toggle();
        StateHasChanged();
    }

    #endregion PublicMethods
}