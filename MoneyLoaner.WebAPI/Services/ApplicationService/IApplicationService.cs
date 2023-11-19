using DebtWeb.WebAPI.Data;
using MoneyLoaner.Data.DTOs;
using System.Collections;

namespace MoneyLoaner.WebAPI.Services.ApplicationService;

public interface IApplicationService
{
    Task<HttpResponse<Hashtable>> SubmitNewProposalAsync(NewProposalDto newProposalDto);
}