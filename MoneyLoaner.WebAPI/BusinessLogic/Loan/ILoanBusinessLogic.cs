using MoneyLoaner.Data.DTOs;
using System.Collections;

namespace MoneyLoaner.WebAPI.BusinessLogic.Loan;

public interface ILoanBusinessLogic
{
    Task<Hashtable> SubmitNewProposalAsync(NewProposalDto newProposalDto);
}