namespace MoneyLoaner.WebAPI.Auth;

public interface ILoginService
{
    public Task LoginAsync(string token, string email);

    public Task LogoutAsync();
}