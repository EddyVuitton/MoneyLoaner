﻿using DebtWeb.WebAPI.Data;
using Microsoft.AspNetCore.Mvc;
using MoneyLoaner.Data.DTOs;
using MoneyLoaner.Data.Forms;
using MoneyLoaner.WebAPI.Data;
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
    public async Task<HttpApiResponseT<UserToken>> Login(LoginAccountForm loginForm)
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
    public async Task<HttpApiResponse> Register(RegisterAccountForm registerForm)
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
    public async Task<HttpApiResponseT<UserAccountDto>> GetUserAccount(string email)
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
    public async Task<HttpApiResponse> UpdateEmailAsync(int pk_id, string email)
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
    public async Task<HttpApiResponse> UpdatePhoneAsync(int pk_id, string phone)
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
    public async Task<HttpApiResponse> UpdatePasswordAsync(UpdatePasswordForm updatePasswordForm)
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