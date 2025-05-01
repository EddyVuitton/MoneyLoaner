using Microsoft.AspNetCore.Components.Authorization;
using MoneyLoaner.WebUI.Auth;
using MoneyLoaner.WebUI.Helpers.Snackbar;
using MoneyLoaner.WebUI.Services.ApplicationService;
using MudBlazor.Services;

namespace MoneyLoaner.Server;

public static class Extensions
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        builder.Services.AddMudServices();
        builder.Services.AddServerServices();
        builder.Services.AddAuthServices();
    }

    public static IServiceCollection AddServerServices(this IServiceCollection services)
    {
        var provider = services.BuildServiceProvider();

        services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:44304/") });
        services.AddScoped<IApplicationService, ApplicationService>();
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