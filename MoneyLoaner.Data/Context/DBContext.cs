using Microsoft.EntityFrameworkCore;
using MoneyLoaner.Data.Helpers;

namespace MoneyLoaner.Data.Context;

public partial class DBContext : DbContext
{
    public DBContext(DbContextOptions<DBContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        DataHelper.AddEntities(modelBuilder);

        OnModelCreatingPartial(modelBuilder);
    }

    private partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}