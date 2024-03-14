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
}