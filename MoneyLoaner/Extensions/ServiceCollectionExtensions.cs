using MoneyLoaner.ComponentsShared.Helpers.Snackbar;
using MoneyLoaner.WebAPI.Services.ApplicationService;

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
}