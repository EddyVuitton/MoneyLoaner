using Microsoft.EntityFrameworkCore.Migrations;
using MoneyLoaner.Domain.Extensions;

#nullable disable

namespace MoneyLoaner.Domain.Migrations;

/// <inheritdoc />
public partial class InitialMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RunSqlScript("tworzenie_bazy_danych");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
    }
}