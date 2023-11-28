using MoneyLoaner.Data.DTOs;

namespace MoneyLoaner.Components.Pages.Auth;

public partial class Account
{
    List<InstallmentDto> _installmentDtos = new();

    protected async override Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        var now = DateTime.UtcNow;

        for (int i = 0; i < 30; i++)
        {
            now = now.AddMonths(1);
            _installmentDtos.Add(new InstallmentDto() { PaymentDate = now });
        }

        _installmentDtos.First().PaymentDate = DateTime.Now;
    }
}