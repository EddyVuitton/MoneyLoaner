using MoneyLoaner.Api.BusinessLogic.Account;
using MoneyLoaner.Api.BusinessLogic.Loan;

namespace MoneyLoaner.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
    {
        services.AddScoped<ILoanBusinessLogic, LoanBusinessLogic>();
        services.AddScoped<IAccountBusinessLogic, AccountBusinessLogic>();

        return services;
    }
}