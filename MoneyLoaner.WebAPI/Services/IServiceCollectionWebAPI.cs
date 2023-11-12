using MoneyLoaner.WebAPI.BusinessLogic.ApplicationRepository;

namespace MoneyLoaner.WebAPI.Services;

public static class IServiceCollectionWebAPI
{
    public static IServiceCollection AddWebAPI(this IServiceCollection services)
    {
        services.AddScoped<IApplicationRepository, ApplicationRepository>();

        return services;
    }
}