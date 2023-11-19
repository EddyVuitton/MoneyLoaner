using Microsoft.AspNetCore.Components.Authorization;
using MoneyLoaner.WebAPI.Auth;
using MoneyLoaner.WebAPI.BusinessLogic.Account;
using MoneyLoaner.WebAPI.BusinessLogic.Loan;

namespace MoneyLoaner.WebAPI.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
    {
        services.AddScoped<ILoanBusinessLogic, LoanBusinessLogic>();
        services.AddScoped<IAccountBusinessLogic, AccountBusinessLogic>();

        return services;
    }

    public static IServiceCollection AddAuthServices(this IServiceCollection services)
    {
        services.AddScoped<JWTAuthenticationStateProvider>();
        services.AddScoped<AuthenticationStateProvider, JWTAuthenticationStateProvider>(provider => provider.GetRequiredService<JWTAuthenticationStateProvider>());
        services.AddScoped<ILoginService, JWTAuthenticationStateProvider>(provider => provider.GetRequiredService<JWTAuthenticationStateProvider>());

        return services;
    }
}