using Microsoft.AspNetCore.Mvc;
using MoneyLoaner.Data.DTOs;
using MoneyLoaner.Data.Http;
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

    [HttpGet("GetScheduleAsync")]
    public async Task<HttpApiResponseT<List<LoanInstallmentDto>>> GetScheduleAsync(int po_id)
    {
        try
        {
            var result = await _businessLogic.GetScheduleAsync(po_id);
            return HttpApiHelper.Ok(result);
        }
        catch (Exception e)
        {
            return HttpApiHelper.Error<List<LoanInstallmentDto>>(e);
        }
    }

    [HttpGet("GetAccountInfoAsync")]
    public async Task<HttpApiResponseT<AccountInfoDto?>> GetAccountInfoAsync(int pk_id)
    {
        try
        {
            var result = await _businessLogic.GetAccountInfoAsync(pk_id);
            return HttpApiHelper.Ok(result);
        }
        catch (Exception e)
        {
            return HttpApiHelper.Error<AccountInfoDto?>(e);
        }
    }

    [HttpGet("GetLoansHistoryAsync")]
    public async Task<HttpApiResponseT<List<LoanHistoryDto>?>> GetLoansHistoryAsync(int pk_id)
    {
        try
        {
            var result = await _businessLogic.GetLoansHistoryAsync(pk_id);
            return HttpApiHelper.Ok(result);
        }
        catch (Exception e)
        {
            return HttpApiHelper.Error<List<LoanHistoryDto>?>(e);
        }
    }

    [HttpGet("GetLoanConfigAsync")]
    public async Task<HttpApiResponseT<LoanConfig?>> GetLoanConfigAsync()
    {
        try
        {
            var result = await _businessLogic.GetLoanConfigAsync();
            return HttpApiHelper.Ok(result);
        }
        catch (Exception e)
        {
            return HttpApiHelper.Error<LoanConfig?>(e);
        }
    }
}