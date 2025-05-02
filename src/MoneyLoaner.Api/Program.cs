using Swashbuckle.AspNetCore.SwaggerUI;
using MoneyLoaner.Domain.Context;
using Microsoft.EntityFrameworkCore;
using MoneyLoaner.Domain.Extensions;
using MoneyLoaner.Api;

var builder = WebApplication.CreateBuilder(args);
builder.AddServices();
builder.HandleDocker();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.DefaultModelsExpandDepth(-1);
    c.DocExpansion(docExpansion: DocExpansion.None);
    c.EnableTryItOutByDefault();
});

await MigrateDatabaseAsync();

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
            Console.WriteLine("Nie można połaczyć się z bazą danych, uruchamiam migrację...");
            await dbContext.Database.MigrateAsync();
            await dbContext.RunSqlScript("tworzenie_bazy_danych");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[MigrateDatabaseAsync] {ex}");
    }
}