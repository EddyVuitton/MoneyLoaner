namespace MoneyLoaner.WebAPI.Auth;

public interface ILoginService
{
    public Task LoginAsync(string token);
    public Task LogoutAsync();
}