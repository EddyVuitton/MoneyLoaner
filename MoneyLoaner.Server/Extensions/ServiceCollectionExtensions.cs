using Microsoft.AspNetCore.Components.Authorization;
using MoneyLoaner.WebUI.Auth;
using MoneyLoaner.WebUI.Helpers.Snackbar;
using MoneyLoaner.WebUI.Services.ApplicationService;

namespace MoneyLoaner.Server.Extentions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServerServices(this IServiceCollection services)
    {
        services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:44304/") });
        services.AddScoped<IApplicationService, ApplicationService>(); //webapi
        services.AddScoped<ISnackbarHelper, SnackbarHelper>();

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