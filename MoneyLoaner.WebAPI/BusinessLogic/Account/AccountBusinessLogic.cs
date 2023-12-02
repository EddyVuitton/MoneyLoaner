using Microsoft.IdentityModel.Tokens;
using MoneyLoaner.Data.Context;
using MoneyLoaner.Data.DTOs;
using MoneyLoaner.Data.Forms;
using MoneyLoaner.Data.Helpers;
using MoneyLoaner.WebAPI.Data;
using MoneyLoaner.WebAPI.Extensions;
using MoneyLoaner.WebAPI.Helpers;
using System.Collections;
using System.Data;
using System.Text;

namespace MoneyLoaner.WebAPI.BusinessLogic.Account;

public class AccountBusinessLogic : IAccountBusinessLogic
{
    private readonly DBContext _context;
    private readonly IConfiguration _configuration;
    private readonly byte[] _jwtKeyBytes;

    public AccountBusinessLogic(DBContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
        _jwtKeyBytes = Encoding.UTF8.GetBytes(_configuration["JWT:key"]!);
    }

    #region PublicMethods

    public async Task<UserToken> LoginAsync(LoginAccountForm loginForm)
    {
        if (loginForm is null || loginForm.Email is null || loginForm.Password is null)
            throw new Exception("Niepoprawna próba logowania");

        var result = false;
        var dbAccountPassword = await GetAccountHashedPasswordAsync(loginForm.Email);
        var hashedPassword = AuthHelper.HashPassword(loginForm.Password);

        if (dbAccountPassword == hashedPassword)
            result = true;

        if (result)
        {
            var key = new SymmetricSecurityKey(_jwtKeyBytes);
            var token = AuthHelper.BuildToken(loginForm.Email, key);

            return token;
        }
        else
        {
            throw new Exception("Nieprawidłowe hasło lub email");
        }
    }

    public async Task<string> RegisterAsync(RegisterAccountForm registerForm)
    {
        if (registerForm is null || string.IsNullOrEmpty(registerForm.Email) || string.IsNullOrEmpty(registerForm.Password))
            throw new Exception("Niepoprawna próba rejestracji");

        var userAccountInfo = await GetUserAccountInfoAsync(registerForm.Email);

        if (userAccountInfo is null)
        {
            var hT = new Hashtable
            {
                { "@pesel", registerForm.PersonalNumber },
                { "@email", registerForm.Email },
                { "@haslo", AuthHelper.HashPassword(registerForm.Password) }
            };

            await SqlHelper.ExecuteSqlQuerySingleAsync("exec p_uzytkownik_konto_dodaj @pesel, @email, @haslo;", hT);

            return "Konto poprawnie zarejestrowane";
        }
        else
        {
            throw new Exception("Istnieje już konto z podanym adresem email");
        }
    }

    public async Task<UserAccountDto?> GetUserAccountInfoAsync(string email)
    {
        return await GetUserAccountInfoStaticAsync(email);
    }

    public async Task UpdateEmailAsync(int pk_id, string email)
    {
        var hT = new object[]
        {
            SqlParam.CreateParameter("pk_id", pk_id, SqlDbType.Int),
            SqlParam.CreateParameter("email", email, SqlDbType.NVarChar),
        };

        await _context.SqlQueryAsync("exec p_klient_email_aktualizuj @pk_id, @email;", hT);
    }

    public async Task UpdatePhoneAsync(int pk_id, string phone)
    {
        var hT = new object[]
        {
            SqlParam.CreateParameter("pk_id", pk_id, SqlDbType.Int),
            SqlParam.CreateParameter("numer_telefonu", phone, SqlDbType.VarChar),
        };

        await _context.SqlQueryAsync("exec p_klient_telefon_aktualizuj @pk_id, @numer_telefonu;", hT);
    }

    public async Task UpdatePasswordAsync(UpdatePasswordForm updatePasswordForm)
    {
        if (updatePasswordForm is null || string.IsNullOrEmpty(updatePasswordForm.Password) || string.IsNullOrEmpty(updatePasswordForm.OldPassword))
            throw new Exception("Niepoprawna próba zmiany hasła");

        var userAccountInfoResult = await GetUserAccountInfoStaticAsync(updatePasswordForm.UserAccountId);

        if (userAccountInfoResult is null)
            throw new Exception("Błąd przy pobraniu danych");

        var oldPassword = AuthHelper.HashPassword(updatePasswordForm.OldPassword!);
        var currentPassword = userAccountInfoResult.Password;
        var newPassword = AuthHelper.HashPassword(updatePasswordForm.Password);

        if (currentPassword != oldPassword)
            throw new Exception("Nieprawidłowe stare hasło");

        var hT = new object[]
        {
            SqlParam.CreateParameter("pk_id", updatePasswordForm.UserAccountId, SqlDbType.Int),
            SqlParam.CreateParameter("haslo", newPassword, SqlDbType.NVarChar),
        };

        await _context.SqlQueryAsync("exec p_uzytkownik_konto_zmien_haslo @pk_id, @haslo;", hT);
    }

    #endregion PublicMethods

    #region PrivateMethods

    private static async Task<string> GetAccountHashedPasswordAsync(string email)
    {
        var password = await GetUserAccountInfoStaticAsync(email);

        if (password is null || password.Password is null)
            return string.Empty;

        return password.Password;
    }

    private static async Task<UserAccountDto?> GetUserAccountInfoStaticAsync(string email)
    {
        var hT = new Hashtable
        {
            { "@email", email }
        };

        var userAccountInfo = await SqlHelper.ExecuteSqlQuerySingleAsync($"exec p_uzytkownik_konto_pobierz @email, null;", hT);

        if (userAccountInfo.Count > 0)
        {
            return DtoHelper.ToUserAccountDto(userAccountInfo);
        }

        return null;
    }

    private static async Task<UserAccountDto?> GetUserAccountInfoStaticAsync(int pk_id)
    {
        var hT = new Hashtable
        {
            { "@pk_id", pk_id }
        };

        var userAccountInfo = await SqlHelper.ExecuteSqlQuerySingleAsync($"exec p_uzytkownik_konto_pobierz null, @pk_id;", hT);

        if (userAccountInfo.Count > 0)
        {
            return DtoHelper.ToUserAccountDto(userAccountInfo);
        }

        return null;
    }

    #endregion PrivateMethods
}