using Microsoft.EntityFrameworkCore;
using MoneyLoaner.Api.Repositories.Account;
using MoneyLoaner.Api.Repositories.Loan;
using MoneyLoaner.Domain.Context;

namespace MoneyLoaner.Api;

public static class Extensions
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddLogging(options =>
        {
            options.AddConsole();
            options.AddDebug();
        });
        builder.Services.AddDbContextFactory<DBContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("Database"));
        });
        builder.Services.AddRepositories();
    }

    public static void HandleDocker(this WebApplicationBuilder builder)
    {
        var envValue = Environment.GetEnvironmentVariable("RanInDocker") ?? "false";

        if (bool.Parse(envValue))
        {
            builder.WebHost.UseUrls("http://0.0.0.0:80");
        }
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ILoanBusinessLogic, LoanBusinessLogic>();
        services.AddScoped<IAccountBusinessLogic, AccountBusinessLogic>();

        return services;
    }
}