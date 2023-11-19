using MoneyLoaner.WebAPI.BusinessLogic.Loan;

namespace MoneyLoaner.WebAPI.Services;

public static class IServiceCollectionWebAPI
{
    public static IServiceCollection AddWebAPI(this IServiceCollection services)
    {
        services.AddScoped<ILoanBusinessLogic, LoanBusinessLogic>();

        return services;
    }
}