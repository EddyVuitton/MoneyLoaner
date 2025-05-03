using MoneyLoaner.Domain.DTOs;

namespace MoneyLoaner.Api.Repositories.Loan;

public interface ILoanBusinessLogic
{
    Task SubmitNewProposalAsync(NewProposalDto newProposalDto);
    Task<IEnumerable<LoanInstallmentDto>> GetScheduleAsync(int po_id);
    Task<AccountInfoDto?> GetAccountInfoAsync(int pk_id);
    Task<IEnumerable<LoanHistoryDto>> GetLoansHistoryAsync(int pk_id);
    Task<LoanConfig?> GetLoanConfigAsync();
}