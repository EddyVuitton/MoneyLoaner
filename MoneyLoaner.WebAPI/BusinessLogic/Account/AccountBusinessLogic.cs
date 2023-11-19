using MoneyLoaner.Data.Context;
using MoneyLoaner.WebAPI.Helpers;
using System.Collections;

namespace MoneyLoaner.WebAPI.BusinessLogic.Account;

public class AccountBusinessLogic : IAccountBusinessLogic
{
    private readonly DBContext _context;

    public AccountBusinessLogic(DBContext context)
    {
        _context = context;
    }



    #region PrivateMethods

    public async Task<string> GetAccountHashedPasswordAsync(string email)
    {
        var hT = new Hashtable
        {
            { "@email", email }
        };

        var password = await SqlHelper.ExecuteSqlQuerySingleAsync("exec [...] @email;", hT);

        return password["haslo"]!.ToString() ?? string.Empty;
    }

    #endregion PrivateMethods
}