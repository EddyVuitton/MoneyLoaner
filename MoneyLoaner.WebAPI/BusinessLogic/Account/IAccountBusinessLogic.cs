using MoneyLoaner.Data.DTOs;
using MoneyLoaner.Data.Forms;
using MoneyLoaner.WebAPI.Data;

namespace MoneyLoaner.WebAPI.BusinessLogic.Account;

public interface IAccountBusinessLogic
{
    Task<UserAccountDto?> GetUserAccountInfoAsync(string email);

    Task<UserToken> LoginAsync(LoginAccountForm loginForm);

    Task<string> RegisterAsync(RegisterAccountForm registerForm);
}