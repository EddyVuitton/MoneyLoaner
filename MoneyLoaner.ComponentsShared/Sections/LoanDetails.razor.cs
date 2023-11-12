using Microsoft.AspNetCore.Components;
using MoneyLoaner.Data.DTOs;

namespace MoneyLoaner.ComponentsShared.Sections;

public partial class LoanDetails
{
    [Parameter] public LoanDto LoanDto { get; set; } = new LoanDto();
    [Parameter] public List<InstallmentDto> InstallmentListDto { get; set; } = new List<InstallmentDto>();

    private readonly DateTime _now = DateTime.Now;
    private string _dateRangeMin = string.Empty;
    private string _dateRangeMax = string.Empty;

    private DateTime? _DayOfDatePayment;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _DayOfDatePayment = LoanDto.FirstInstallmentPaymentDate;
        _dateRangeMin = new DateTime(_now.Year, _now.Month, 1)
            .AddMonths(1)
            .AddDays(-1)
            .AddDays(-3)
            .Date.ToString("yyyy-MM-dd");
        _dateRangeMax = new DateTime(_now.AddMonths(1).Year, _now.AddMonths(1).Month, 1)
            .AddMonths(1)
            .AddDays(-1)
            .AddDays(-4)
            .Date.ToString("yyyy-MM-dd");
    }

    protected override async Task OnAfterRenderAsync(bool b)
    {
        await base.OnAfterRenderAsync(b);

        if (_DayOfDatePayment is null)
        {
            _DayOfDatePayment = LoanDto.FirstInstallmentPaymentDate;
            StateHasChanged();
        }
    }
}