using MoneyLoaner.Domain.Auth;
using MoneyLoaner.Domain.DTOs;
using MoneyLoaner.Domain.Forms;

namespace MoneyLoaner.WebAPI.BusinessLogic.Account;

public interface IAccountBusinessLogic
{
    Task<UserAccountDto?> GetUserAccountInfoAsync(string email = "", int pk_id = 0, string pesel = "");
    Task<UserToken> LoginAsync(LoginAccountForm loginForm);
    Task<string> RegisterAsync(RegisterAccountForm registerForm);
    Task UpdateEmailAsync(int pk_id, string email);
    Task UpdatePhoneAsync(int pk_id, string phone);
    Task UpdatePasswordAsync(UpdatePasswordForm updatePasswordForm);
}