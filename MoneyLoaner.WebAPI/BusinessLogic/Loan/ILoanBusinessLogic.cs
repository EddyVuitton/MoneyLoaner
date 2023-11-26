using MoneyLoaner.Data.DTOs;

namespace MoneyLoaner.WebAPI.BusinessLogic.Loan;

public interface ILoanBusinessLogic
{
    Task SubmitNewProposalAsync(NewProposalDto newProposalDto);
}