using MoneyLoaner.Data.Auth;

namespace MoneyLoaner.WebUI.Auth;

public interface ILoginService
{
    Task LoginAsync(UserToken userToken);
    Task LogoutAsync();
    Task<int> IsLoggedInAsync();
    Task LogoutIfExpiredTokenAsync();
}