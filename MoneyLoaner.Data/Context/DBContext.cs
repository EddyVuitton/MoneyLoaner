﻿using Microsoft.EntityFrameworkCore;
using MoneyLoaner.Data.DTOs;
using MoneyLoaner.Data.Helpers;

namespace MoneyLoaner.Data.Context;

public partial class DBContext : DbContext
{
    public DBContext(DbContextOptions<DBContext> options) : base(options)
    {
    }

    public virtual DbSet<TestModelDto> TestModelDto { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        DataHelper.AddEntities(modelBuilder);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}