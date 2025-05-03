using Microsoft.AspNetCore.Mvc;
using MoneyLoaner.Domain.Auth;
using MoneyLoaner.Domain.DTOs;
using MoneyLoaner.Domain.Forms;

namespace MoneyLoaner.Api.Repositories.Account;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly IAccountBusinessLogic _businessLogic;

    public AccountController(ILogger<AccountController> logger, IAccountBusinessLogic businessLogic)
    {
        _logger = logger;
        _businessLogic = businessLogic;
    }

    [HttpPost("Login")]
    public async Task<ActionResult<UserToken>> Login(LoginAccountForm loginForm)
    {
        try
        {
            var result = await _businessLogic.LoginAsync(loginForm);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPost("Register")]
    public async Task<ActionResult<string>> Register(RegisterAccountForm registerForm)
    {
        try
        {
            var result = await _businessLogic.RegisterAsync(registerForm);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpGet("GetUserAccount")]
    public async Task<ActionResult<UserAccountDto>> GetUserAccount(string email)
    {
        try
        {
            var result = await _businessLogic.GetUserAccountInfoAsync(email);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPost("UpdateEmailAsync")]
    public async Task<ActionResult> UpdateEmailAsync(int pk_id, string email)
    {
        try
        {
            await _businessLogic.UpdateEmailAsync(pk_id, email);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPost("UpdatePhoneAsync")]
    public async Task<ActionResult> UpdatePhoneAsync(int pk_id, string phone)
    {
        try
        {
            await _businessLogic.UpdatePhoneAsync(pk_id, phone);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPost("UpdatePasswordAsync")]
    public async Task<ActionResult> UpdatePasswordAsync(UpdatePasswordForm updatePasswordForm)
    {
        try
        {
            await _businessLogic.UpdatePasswordAsync(updatePasswordForm);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
}