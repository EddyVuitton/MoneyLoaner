using Microsoft.EntityFrameworkCore;
using MoneyLoaner.Domain.Extensions;

namespace MoneyLoaner.Domain.Context;

public partial class DBContext(DbContextOptions<DBContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.AddDtos();

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}