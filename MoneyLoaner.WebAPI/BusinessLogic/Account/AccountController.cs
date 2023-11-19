using DebtWeb.WebAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MoneyLoaner.WebAPI.Data;
using MoneyLoaner.WebAPI.Helpers;
using PI6.Shared.Data.Dtos;
using System.Text;

namespace MoneyLoaner.WebAPI.BusinessLogic.Account;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly IAccountBusinessLogic _businessLogic;
    private readonly IConfiguration _configuration;
    private readonly byte[] _jwtKeyBytes;

    public AccountController(ILogger<AccountController> logger, IAccountBusinessLogic businessLogic, IConfiguration configuration)
    {
        try
        {
            _logger = logger;
            _businessLogic = businessLogic;
            _configuration = configuration;
            _jwtKeyBytes = Encoding.UTF8.GetBytes(_configuration["JWT:key"]!);
        }
        catch
        {
            throw new Exception("Nie udana próba zainicjalizowania");
        }
    }

    [HttpPost("Login")]
    public async Task<HttpResponse<UserToken>> Login(AccountDto account)
    {
        if (account is null || account.UserEmail is null || account.UserPassword is null)
            return HttpHelper.Error<UserToken>(new Exception("Niepoprawna próba logowania"));

        var result = false;
        var dbAccountPassword = await _businessLogic.GetAccountHashedPasswordAsync(account.UserEmail);
        var hashedPassword = AuthHelper.HashPassword(account.UserPassword);

        if (dbAccountPassword == hashedPassword)
            result = true;

        if (result)
        {
            try
            {
                var key = new SymmetricSecurityKey(_jwtKeyBytes);
                var token = AuthHelper.BuildToken(account.UserEmail, key);

                return HttpHelper.Ok(token);
            }
            catch (Exception e)
            {
                return HttpHelper.Error<UserToken>(e);
            }
        }
        else
        {
            return HttpHelper.Error<UserToken>(new Exception("Nie prawidłowe hasło"));
        }
    }
}