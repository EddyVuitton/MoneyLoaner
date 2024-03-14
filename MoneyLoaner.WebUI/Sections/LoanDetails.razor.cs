using Microsoft.AspNetCore.Components;
using MoneyLoaner.Data.DTOs;

namespace MoneyLoaner.WebUI.Sections;

public partial class LoanDetails
{
    [Parameter] public LoanDto LoanDto { get; set; } = new LoanDto();
    [Parameter] public List<InstallmentDto> InstallmentListDto { get; set; } = new List<InstallmentDto>();
    [Parameter] public Func<DateTime?, Task>? ParamChangeDatePayment { get; set; }

    private readonly DateTime _now = DateTime.Now;
    private string _dateRangeMin = string.Empty;
    private string _dateRangeMax = string.Empty;

    private DateTime? _DayOfDatePayment;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        InitFields();
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

    private void OnChange(ChangeEventArgs e)
    {
        if (e.Value is not null && ParamChangeDatePayment is not null)
        {
            if (string.IsNullOrEmpty(e.Value.ToString()))
            {
                _DayOfDatePayment = null;
                return;
            }

            _DayOfDatePayment = Convert.ToDateTime(e.Value);
            ParamChangeDatePayment.Invoke(_DayOfDatePayment);
        }
    }

    private void InitFields()
    {
        _DayOfDatePayment = _now;
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
}