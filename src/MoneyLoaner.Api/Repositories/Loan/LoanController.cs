using Microsoft.AspNetCore.Mvc;
using MoneyLoaner.Domain.DTOs;

namespace MoneyLoaner.Api.Repositories.Loan;

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
    public async Task<ActionResult> SubmitNewProposalAsync(NewProposalDto newProposalDto)
    {
        try
        {
            await _businessLogic.SubmitNewProposalAsync(newProposalDto);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpGet("GetScheduleAsync")]
    public async Task<ActionResult<List<LoanInstallmentDto>>> GetScheduleAsync(int po_id)
    {
        try
        {
            var result = await _businessLogic.GetScheduleAsync(po_id);

            return Ok(result.ToList());
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpGet("GetAccountInfoAsync")]
    public async Task<ActionResult<AccountInfoDto>> GetAccountInfoAsync(int pk_id)
    {
        try
        {
            var result = await _businessLogic.GetAccountInfoAsync(pk_id);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpGet("GetLoansHistoryAsync")]
    public async Task<ActionResult<List<LoanHistoryDto>>> GetLoansHistoryAsync(int pk_id)
    {
        try
        {
            var result = await _businessLogic.GetLoansHistoryAsync(pk_id);

            return Ok(result.ToList());
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpGet("GetLoanConfigAsync")]
    public async Task<ActionResult<LoanConfig?>> GetLoanConfigAsync()
    {
        try
        {
            var result = await _businessLogic.GetLoanConfigAsync();

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
}