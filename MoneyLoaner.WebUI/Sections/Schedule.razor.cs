using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MoneyLoaner.Domain.DTOs;

namespace MoneyLoaner.WebUI.Sections;

public partial class Schedule
{
    [Parameter] public List<LoanInstallmentDto>? InstallmentDtos { get; set; }
}