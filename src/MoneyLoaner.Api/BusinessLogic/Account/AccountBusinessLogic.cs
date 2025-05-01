using Microsoft.IdentityModel.Tokens;
using MoneyLoaner.Domain.Auth;
using MoneyLoaner.Domain.DTOs;
using MoneyLoaner.Domain.Forms;
using MoneyLoaner.Api.Helpers;
using System.Text;
using Dapper;
using Microsoft.Data.SqlClient;

namespace MoneyLoaner.Api.BusinessLogic.Account;

public class AccountBusinessLogic(IConfiguration configuration) : IAccountBusinessLogic
{
    private readonly string _connectionString = configuration.GetConnectionString("Database") ?? throw new Exception("Brak connection string do bazy danych");
    private readonly byte[] _jwtKeyBytes = Encoding.UTF8.GetBytes(configuration["JWT:key"]!);

    public async Task<UserToken> LoginAsync(LoginAccountForm loginForm)
    {
        if (loginForm is null || loginForm.Email is null || loginForm.Password is null)
            throw new Exception("Niepoprawna próba logowania");

        var result = false;
        var dbAccountPassword = (await GetUserAccountInfoAsync(email: loginForm.Email))?.Password;
        var hashedPassword = AuthHelper.HashPassword(loginForm.Password);

        if (dbAccountPassword == hashedPassword)
            result = true;

        if (result)
        {
            var userAccountInfo = await GetUserAccountInfoAsync(pesel: loginForm.PersonalNumber!);

            var key = new SymmetricSecurityKey(_jwtKeyBytes);
            var token = AuthHelper.BuildToken(loginForm.Email, userAccountInfo!.LoanCustomerId, key);

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

        await using var con = new SqlConnection(_connectionString);

        var param = new
        {
            imie = registerForm.Name,
            nazwisko = registerForm.Surname,
            pesel = registerForm.PersonalNumber,
            email = registerForm.Email,
            haslo = AuthHelper.HashPassword(registerForm.Password)
        };
        await con.QueryAsync("exec p_uzytkownik_konto_dodaj @imie, @nazwisko, @pesel, @email, @haslo;", param);

        return "Konto poprawnie zarejestrowane";
    }

    public async Task UpdateEmailAsync(int pk_id, string email)
    {
        await using var con = new SqlConnection(_connectionString);

        var userAccountInfo = await GetUserAccountInfoAsync(email: email);

        if (userAccountInfo is null)
        {
            var param = new
            {
                pk_id,
                email
            };
            await con.QueryAsync("exec p_klient_email_aktualizuj @pk_id, @email;", param);
        }
        else
        {
            throw new Exception("Istnieje już konto z podanym adresem email");
        }
    }

    public async Task UpdatePhoneAsync(int pk_id, string phone)
    {
        await using var con = new SqlConnection(_connectionString);

        var param = new
        {
            pk_id,
            numer_telefonu = phone
        };
        await con.QueryAsync("exec p_klient_telefon_aktualizuj @pk_id, @haslo;", param);
    }

    public async Task UpdatePasswordAsync(UpdatePasswordForm updatePasswordForm)
    {
        if (updatePasswordForm is null || string.IsNullOrEmpty(updatePasswordForm.Password) || string.IsNullOrEmpty(updatePasswordForm.OldPassword))
            throw new Exception("Niepoprawna próba zmiany hasła");

        var userAccountInfoResult = await GetUserAccountInfoAsync(pk_id: updatePasswordForm.UserAccountId);

        if (userAccountInfoResult is null)
            throw new Exception("Błąd przy pobraniu danych");

        var oldPassword = AuthHelper.HashPassword(updatePasswordForm.OldPassword!);
        var currentPassword = userAccountInfoResult.Password;
        var newPassword = AuthHelper.HashPassword(updatePasswordForm.Password);

        if (currentPassword != oldPassword)
            throw new Exception("Nieprawidłowe stare hasło");

        await using var con = new SqlConnection(_connectionString);

        var param = new
        {
            pk_id = updatePasswordForm.UserAccountId,
            haslo = newPassword
        };
        await con.QueryAsync("exec p_uzytkownik_konto_zmien_haslo @pk_id, @haslo;", param);
    }

    public async Task<UserAccountDto?> GetUserAccountInfoAsync(string email = "", int pk_id = 0, string pesel = "")
    {
        await using var con = new SqlConnection(_connectionString);

        var param = new { email, pk_id, pesel };
        var result = await con.QuerySingleAsync<UserAccountDto>($"exec p_uzytkownik_konto_pobierz @email, @pk_id, @pesel;", param);

        return result;
    }
}