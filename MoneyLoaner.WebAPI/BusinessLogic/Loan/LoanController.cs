using DebtWeb.WebAPI.Data;
using Microsoft.AspNetCore.Mvc;
using MoneyLoaner.Data.DTOs;
using MoneyLoaner.WebAPI.Helpers;

namespace MoneyLoaner.WebAPI.BusinessLogic.Loan;

[ApiController]
[Route("api/[controller]")]
public class LoanController : ControllerBase
{
    private readonly ILogger<LoanController> _logger;
    private readonly ILoanBusinessLogic _businessLogic;
    private readonly IConfiguration _configuration;

    public LoanController(ILogger<LoanController> logger, ILoanBusinessLogic businessLogic, IConfiguration configuration)
    {
        _logger = logger;
        _businessLogic = businessLogic;
        _configuration = configuration;
    }

    [HttpPost("SubmitNewProposalAsync")]
    public async Task<HttpApiResponse> SubmitNewProposalAsync(NewProposalDto newProposalDto)
    {
        try
        {
            await _businessLogic.SubmitNewProposalAsync(newProposalDto);
            return HttpApiHelper.Ok();
        }
        catch (Exception e)
        {
            return HttpApiHelper.Error(e);
        }
    }
}