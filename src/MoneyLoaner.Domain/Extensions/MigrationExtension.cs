using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using MoneyLoaner.Domain.Context;
using MoneyLoaner.Domain.DTOs;

namespace MoneyLoaner.Domain.Extensions;

public static class MigrationExtensions
{
    public static async Task RunSqlScript(this DBContext context, string fileName)
    {
        var assemblyLocation = Assembly.GetExecutingAssembly().Location;
        string path = Path.Combine(Path.GetDirectoryName(assemblyLocation)!, @$"Context/{fileName}.sql");

        using var stream = new StreamReader(path);
        var script = stream.ReadToEnd();
        var canConnect = await context.Database.CanConnectAsync();
        
        if (context is null || !canConnect)
        {
            return;
        }

        var batches = Regex.Split(script, @"\bgo\b");

        foreach (var batch in batches)
        {
            if (string.IsNullOrEmpty(batch))
            {
                continue;
            }

            await context.Database.ExecuteSqlRawAsync(batch);
        }
    }

    public static void AddDtos(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LoanInstallmentDto>(entity =>
        {
            entity.HasNoKey();
        });

        modelBuilder.Entity<AccountInfoDto>(entity =>
        {
            entity.HasNoKey();
        });

        modelBuilder.Entity<LoanHistoryDto>(entity =>
        {
            entity.HasNoKey();
        });

        modelBuilder.Entity<LoanConfig>(entity =>
        {
            entity.HasNoKey();
        });
    }
}