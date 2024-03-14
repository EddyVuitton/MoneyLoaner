using Microsoft.EntityFrameworkCore.Migrations;
using System.Reflection;

namespace MoneyLoaner.Domain.Extensions;

public static class MigrationExtensions
{
    public static void RunSqlScript(this MigrationBuilder migrationBuilder, string script)
    {
        var assemblyLocation = Assembly.GetExecutingAssembly().Location;
        string path = Path.Combine(Path.GetDirectoryName(assemblyLocation)!, $"Context\\{script}.sql");

        using var stream = new StreamReader(path);
        var sqlResult = stream.ReadToEnd();

        if (!string.IsNullOrEmpty(sqlResult))
            migrationBuilder.Sql(sqlResult);
    }
}