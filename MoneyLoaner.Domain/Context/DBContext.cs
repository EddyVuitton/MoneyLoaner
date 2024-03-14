using Microsoft.EntityFrameworkCore;
using MoneyLoaner.Domain.Helpers;

namespace MoneyLoaner.Domain.Context;

public partial class DBContext : DbContext
{
    public DBContext(DbContextOptions<DBContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        DataHelper.AddEntities(modelBuilder);
        DataHelper.AddDtos(modelBuilder);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}