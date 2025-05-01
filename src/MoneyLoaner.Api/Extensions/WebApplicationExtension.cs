using Microsoft.EntityFrameworkCore;
using MoneyLoaner.Domain.Context;

namespace MoneyLoaner.Api.Extensions;

public static class WebApplicationExtension
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
        builder.Services.AddBusinessLogic();
    }
}