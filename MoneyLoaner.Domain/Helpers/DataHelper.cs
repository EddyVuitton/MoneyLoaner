using Microsoft.EntityFrameworkCore;
using MoneyLoaner.Domain.DTOs;

namespace MoneyLoaner.Domain.Helpers;

public static class DataHelper
{
    public static void AddEntities(this ModelBuilder modelBuilder)
    {
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