using Microsoft.EntityFrameworkCore;
using MoneyLoaner.Domain.Context;

namespace MoneyLoaner.WebAPI.Extensions;

public static class WebApplicationExtension
{
    public static void ReMigrateDatabase(this WebApplication app)
    {
        using var serviceScope = app.Services.CreateScope();
        var context = serviceScope.ServiceProvider.GetService<DBContext>()!;

        context.Database.EnsureDeleted(); //zdropuj bazę danych, jeżeli istnieje...
        context.Database.Migrate(); //i stwórz ją razem z jej obiektami
    }
}