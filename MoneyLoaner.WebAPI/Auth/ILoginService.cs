using MoneyLoaner.WebAPI.Data;

namespace MoneyLoaner.WebAPI.Auth;

public interface ILoginService
{
    Task LoginAsync(UserToken userToken);

    Task LogoutAsync();

    Task<int> IsLoggedInAsync();

    Task LogoutIfExpiredTokenAsync();
}