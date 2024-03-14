using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MoneyLoaner.Data.DTOs;

namespace MoneyLoaner.WebUI.Sections;

public partial class Schedule
{
#nullable disable
    [Inject] public IJSRuntime JS { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
#nullable enable

    [Parameter] public List<LoanInstallmentDto>? InstallmentDtos { get; set; }
}