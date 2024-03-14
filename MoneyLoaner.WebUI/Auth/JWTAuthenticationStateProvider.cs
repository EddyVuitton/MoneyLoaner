using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using MoneyLoaner.Data.Auth;
using MoneyLoaner.WebUI.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace MoneyLoaner.WebUI.Auth;

public class JWTAuthenticationStateProvider : AuthenticationStateProvider, ILoginService
{
    //https://www.udemy.com/course/programming-in-blazor-aspnet-core/learn/lecture/17136788#overview
    private readonly IJSRuntime _js;

    private readonly HttpClient _httpClient;
    private const string _TOKENKEY = "TOKENKEY";
    private static AuthenticationState _anonymous => new(new ClaimsPrincipal(new ClaimsIdentity()));

    public JWTAuthenticationStateProvider(IJSRuntime js, HttpClient httpClient)
    {
        _js = js;
        _httpClient = httpClient;
    }

    #region PublicMethods

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var token = await _js.GetFromLocalStorage(_TOKENKEY);

            if (string.IsNullOrEmpty(token))
                return _anonymous;

            return BuildAuthenticationState(token);
        }
        catch
        {
            return await Task.FromResult(_anonymous);
        }
    }

    public AuthenticationState BuildAuthenticationState(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt")));
    }

    public async Task<int> IsLoggedInAsync()
    {
        var authenticationState = await this.GetAuthenticationStateAsync();

        if (authenticationState is not null && authenticationState.User.Claims.Any())
        {
            var result = authenticationState.User.Claims.FirstOrDefault(x => x.Type == "ClientId")?.Value;
            if (int.TryParse(result, out int userAccountId))
            {
                return userAccountId;
            }
        }

        return -1;
    }

    public async Task LogoutIfExpiredTokenAsync()
    {
        var now = DateTime.Now;
        var validTo = await this.TokenValidToAsync();

        if (validTo.CompareTo(now) <= 0)
        {
            await this.LogoutAsync();
        }
    }

    public async Task LoginAsync(UserToken userToken)
    {
        await _js.SetInLocalStorage(_TOKENKEY, userToken.Token);
        var authState = BuildAuthenticationState(userToken.Token);
        NotifyAuthenticationStateChanged(Task.FromResult(authState));
    }

    public async Task LogoutAsync()
    {
        await CleanUpAsync();
    }

    #endregion PublicMethods

    #region PrivateMethods

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        keyValuePairs!.TryGetValue(ClaimTypes.Role, out var roles);

        if (roles is not null)
        {
            if (roles.ToString()!.Trim().StartsWith("["))
            {
                var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString()!);

                if (parsedRoles is null)
                    return new List<Claim>();

                foreach (var parsedRole in parsedRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                }
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, roles.ToString()!));
            }

            keyValuePairs.Remove(ClaimTypes.Role);
        }

        claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!)));
        return claims;
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }

    private async Task CleanUpAsync()
    {
        await _js.RemoveItemFromLocalStorage(_TOKENKEY);
        _httpClient.DefaultRequestHeaders.Authorization = null;
        NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
    }

    private async Task<DateTime> TokenValidToAsync()
    {
        var token = await _js.GetFromLocalStorage(_TOKENKEY);

        if (!string.IsNullOrEmpty(token))
        {
            var validTo = new JwtSecurityTokenHandler().ReadToken(token).ValidTo.ToLocalTime();

            return validTo;
        }

        return DateTime.Now;
    }

    #endregion PrivateMethods
}