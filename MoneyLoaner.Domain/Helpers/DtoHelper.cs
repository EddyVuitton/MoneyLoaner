using MoneyLoaner.Domain.DTOs;
using System.Collections;

namespace MoneyLoaner.Domain.Helpers;

public static class DtoHelper
{
    public static UserAccountDto ToUserAccountDto(Hashtable ht)
    {
        var userAccountDto = new UserAccountDto
        {
            Id = int.Parse(ht["uk_id"]!.ToString()!),
            Email = ht["uk_email"]!.ToString()!,
            Password = ht["uk_haslo"]!.ToString()!,
            DateOfCreate = DateTime.Parse(ht["uk_data_dodania"]!.ToString()!),
            IsActive = bool.Parse(ht["uk_czy_aktywne"]!.ToString()!),
            LoanCustomerId = int.Parse(ht["uk_pk_id"]!.ToString()!)
        };

        return userAccountDto;
    }
}