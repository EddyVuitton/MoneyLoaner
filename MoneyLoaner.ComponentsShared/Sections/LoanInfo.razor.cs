using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MoneyLoaner.ComponentsShared.Dialogs.Auth;
using MoneyLoaner.ComponentsShared.Helpers.Snackbar;
using MoneyLoaner.ComponentsShared.Subsections;
using MoneyLoaner.Data.DTOs;
using MoneyLoaner.WebAPI.Auth;
using MoneyLoaner.WebAPI.Services.ApplicationService;
using MudBlazor;

namespace MoneyLoaner.ComponentsShared.Sections;

public partial class LoanInfo
{
#nullable disable
    [Inject] public IApplicationService ApplicationService { get; set; }
    [Inject] public ISnackbarHelper SnackbarHelper { get; set; }
    [Inject] public IJSRuntime JS { get; set; }
    [Inject] public ILoginService LoginService { get; set; }
    [Inject] public IDialogService DialogService { get; set; }
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

    #region PrivateMethods

    private async Task ChangeDatePayment(DateTime? day)
    {
        if (day is not null)
        {
            _loan.DayOfDatePayment = day.Value.Day;
            _basicLoanDataRef.CalculateInstallments();

            StateHasChanged();
        }
    }

    private void OpenLoginDialog()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = false,
            NoHeader = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        };

        DialogService.Show<LoginDialog>(string.Empty, options);
    }

    private void OpenRegisterDialog(ProposalDto proposal)
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = false,
            NoHeader = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        };

        var parameters = new DialogParameters<ProposalDto>
        {
            { "Proposal", proposal }
        };

        DialogService.Show<RegisterDialog>(string.Empty, parameters, options);
    }

    private void ToggleLoading()
    {
        _submitProposalLoading = !_submitProposalLoading;
        StateHasChanged();
    }

    #endregion PrivateMethods

    #region PublicMethods

    public async Task SubmitNewProposal(ProposalDto proposalDto)
    {
        ToggleLoading();

        await Task.Delay(1500);

        _newProposalDto = new() { LoanDto = _loan, ProposalDto = proposalDto };

        try
        {
            var newProposal = await ApplicationService.SubmitNewProposalAsync(_newProposalDto);

            if (!newProposal.IsSucces)
            {
                throw new Exception(newProposal.Message!);
            }

            SnackbarHelper.Show("Wniosek został przetworzony", Severity.Success, true, false);

            var customerInfo = await ApplicationService.GetUserAccountAsync(proposalDto.Email!);

            if (customerInfo.IsSuccess && customerInfo.Data is not null)
            {
                var clientId = await LoginService.IsLoggedInAsync();

                if (clientId < 0)
                {
                    OpenLoginDialog();
                    SnackbarHelper.Show("Zaloguj się, aby zobaczyć decyzję złożonego wniosku", Severity.Info, false, false);
                }
                else
                {
                    SnackbarHelper.Show("Na stronie Twojego konta pojawiła się informacja o decyzji nowej pożyczki", Severity.Info, false, false);
                }
            }
            else
            {
                OpenRegisterDialog(proposalDto);
                SnackbarHelper.Show("Stwórz teraz swoje konto, aby zobaczyć decyzję złożonego wniosku", Severity.Success, false, false);
            }
        }
        catch (Exception ex)
        {
            SnackbarHelper.Show(ex.Message, Severity.Error);
        }
        finally
        {
            ToggleLoading();
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