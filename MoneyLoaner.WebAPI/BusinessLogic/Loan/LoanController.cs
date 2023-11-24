using DebtWeb.WebAPI.Data;
using Microsoft.AspNetCore.Mvc;
using MoneyLoaner.Data.DTOs;
using MoneyLoaner.WebAPI.Helpers;
using System.Collections;

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
    public async Task<HttpApiResponseT<Hashtable>> SubmitNewProposalAsync(NewProposalDto newProposalDto)
    {
        try
        {
            var result = await _businessLogic.SubmitNewProposalAsync(newProposalDto);
            return HttpHelper.Ok(result);
        }
        catch (Exception e)
        {
            return HttpHelper.Error<Hashtable>(e);
        }
    }

    [HttpPost("Test")]
    public async Task<HttpApiResponse> Test(int x)
    {
        try
        {
            //var y = 1 / (x - x);

            return HttpHelper.Ok();
        }
        catch (Exception ex)
        {
            return HttpHelper.Error(ex);
        }
    }
}