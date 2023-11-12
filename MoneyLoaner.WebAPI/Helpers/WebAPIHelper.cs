using MoneyLoaner.Data.Context;
using MoneyLoaner.WebAPI.Services;

namespace MoneyLoaner.WebAPI.Helpers;

public static class WebAPIHelper
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddWebAPI();
        builder.Services.AddSqlServer<DBContext>(builder.Configuration.GetConnectionString("LogicDatabaseConnection"));
    }
}