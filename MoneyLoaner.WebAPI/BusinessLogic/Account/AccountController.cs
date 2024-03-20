using Microsoft.AspNetCore.Mvc;
using MoneyLoaner.Domain.Auth;
using MoneyLoaner.Domain.DTOs;
using MoneyLoaner.Domain.Forms;
using MoneyLoaner.Domain.Http;
using MoneyLoaner.WebAPI.Helpers;

namespace MoneyLoaner.WebAPI.BusinessLogic.Account;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly IAccountBusinessLogic _businessLogic;
    private readonly IConfiguration _configuration;

    public AccountController(ILogger<AccountController> logger, IAccountBusinessLogic businessLogic, IConfiguration configuration)
    {
        _logger = logger;
        _businessLogic = businessLogic;
        _configuration = configuration;
    }

    [HttpPost("Login")]
    public async Task<HttpResultT<UserToken>> Login(LoginAccountForm loginForm)
    {
        try
        {
            var result = await _businessLogic.LoginAsync(loginForm);
            return HttpApiHelper.Ok(result);
        }
        catch (Exception e)
        {
            return HttpApiHelper.Error<UserToken>(e);
        }
    }

    [HttpPost("Register")]
    public async Task<HttpResult> Register(RegisterAccountForm registerForm)
    {
        try
        {
            var result = await _businessLogic.RegisterAsync(registerForm);
            return HttpApiHelper.Ok(result);
        }
        catch (Exception e)
        {
            return HttpApiHelper.Error(e);
        }
    }

    [HttpGet("GetUserAccount")]
    public async Task<HttpResultT<UserAccountDto>> GetUserAccount(string email)
    {
        try
        {
            var result = await _businessLogic.GetUserAccountInfoAsync(email);

            if (result is null)
            {
                throw new Exception("Brak użytkownika w bazie");
            }

            return HttpApiHelper.Ok(result);
        }
        catch (Exception e)
        {
            return HttpApiHelper.Error<UserAccountDto>(e);
        }
    }

    [HttpPost("UpdateEmailAsync")]
    public async Task<HttpResult> UpdateEmailAsync(int pk_id, string email)
    {
        try
        {
            await _businessLogic.UpdateEmailAsync(pk_id, email);
            return HttpApiHelper.Ok();
        }
        catch (Exception e)
        {
            return HttpApiHelper.Error(e);
        }
    }

    [HttpPost("UpdatePhoneAsync")]
    public async Task<HttpResult> UpdatePhoneAsync(int pk_id, string phone)
    {
        try
        {
            await _businessLogic.UpdatePhoneAsync(pk_id, phone);
            return HttpApiHelper.Ok();
        }
        catch (Exception e)
        {
            return HttpApiHelper.Error(e);
        }
    }

    [HttpPost("UpdatePasswordAsync")]
    public async Task<HttpResult> UpdatePasswordAsync(UpdatePasswordForm updatePasswordForm)
    {
        try
        {
            await _businessLogic.UpdatePasswordAsync(updatePasswordForm);
            return HttpApiHelper.Ok();
        }
        catch (Exception e)
        {
            return HttpApiHelper.Error(e);
        }
    }
}