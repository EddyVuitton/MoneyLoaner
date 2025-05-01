using MoneyLoaner.Api.Extensions;
using Swashbuckle.AspNetCore.SwaggerUI;
using MoneyLoaner.Domain.Context;
using Microsoft.EntityFrameworkCore;
using MoneyLoaner.Domain.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServices();

if (builder.Environment.IsProduction())
{
    //Na potrzeby Dockera
    builder.WebHost.UseUrls("http://0.0.0.0:80");
}

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.DefaultModelsExpandDepth(-1);
    c.DocExpansion(docExpansion: DocExpansion.None);
    c.EnableTryItOutByDefault();
});

await MigrateDatabaseAsync();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

async Task MigrateDatabaseAsync()
{
    try
    {
        using var serviceScope = app.Services.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetService<DBContext>();

        if (dbContext is null)
        {
            return;
        }

        var canConnect = await dbContext.Database.CanConnectAsync();

        if (!canConnect)
        {
            await dbContext.Database.MigrateAsync();
            await dbContext.RunSqlScript("tworzenie_bazy_danych");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[MigrateDatabaseAsync] {ex}");
    }
}