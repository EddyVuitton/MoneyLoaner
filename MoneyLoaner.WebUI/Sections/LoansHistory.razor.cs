using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MoneyLoaner.Domain.DTOs;

namespace MoneyLoaner.WebUI.Sections;

public partial class LoansHistory
{
#nullable disable
    [Inject] public IJSRuntime JS { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
#nullable enable

    [Parameter] public List<LoanHistoryDto>? LoanHistoryDtos { get; set; }
}