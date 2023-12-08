﻿using MoneyLoaner.Data.DTOs;

namespace MoneyLoaner.WebAPI.BusinessLogic.Loan;

public interface ILoanBusinessLogic
{
    Task SubmitNewProposalAsync(NewProposalDto newProposalDto);
    Task<List<LoanInstallmentDto>> GetScheduleAsync(int pd_id);
    Task<AccountInfoDto?> GetAccountInfoAsync(int pk_id);
}