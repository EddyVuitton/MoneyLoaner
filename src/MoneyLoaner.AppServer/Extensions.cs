using Microsoft.AspNetCore.Components.Authorization;
using MoneyLoaner.WebUI.Auth;
using MoneyLoaner.WebUI.Helpers.Snackbar;
using MoneyLoaner.WebUI.Services.ApplicationService;
using MudBlazor.Services;

namespace MoneyLoaner.AppServer;

public static class Extensions
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorComponents().AddInteractiveServerComponents();
        builder.Services.AddMudServices();
        builder.AddAuthServices();
        builder.AddApi();
    }

    public static void HandleDocker(this WebApplicationBuilder builder)
    {
        var envValue = Environment.GetEnvironmentVariable("RanInDocker") ?? "false";

        if (bool.Parse(envValue))
        {
            builder.WebHost.UseUrls("http://0.0.0.0:80");
        }
    }

    private static void AddApi(this WebApplicationBuilder builder)
    {
        var apiBaseAddress = builder.Configuration.GetSection("Api:BaseAddress").Value ?? string.Empty;

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseAddress) });
        builder.Services.AddScoped<IApplicationService, ApplicationService>();
        builder.Services.AddScoped<ISnackbarHelper, SnackbarHelper>();
    }

    private static void AddAuthServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<JWTAuthenticationStateProvider>();
        builder.Services.AddScoped<AuthenticationStateProvider, JWTAuthenticationStateProvider>(provider => provider.GetRequiredService<JWTAuthenticationStateProvider>());
        builder.Services.AddScoped<ILoginService, JWTAuthenticationStateProvider>(provider => provider.GetRequiredService<JWTAuthenticationStateProvider>());
    }
}