namespace MoneyLoaner.WebAPI.BusinessLogic.Account;

public interface IAccountBusinessLogic
{
    Task<string> GetAccountHashedPasswordAsync(string email);
}