using MoneyLoaner.Data.DTOs;
using MoneyLoaner.Data.Helpers;
using Microsoft.EntityFrameworkCore;

namespace MoneyLoaner.Data.Context;

public partial class DBContext : DbContext
{
    public DBContext(DbContextOptions<DBContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        DataHelper.AddEntities(modelBuilder);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}