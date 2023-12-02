using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MoneyLoaner.ComponentsShared.Helpers.Snackbar;
using MoneyLoaner.Data.DTOs;
using MoneyLoaner.WebAPI.Services.ApplicationService;

namespace MoneyLoaner.ComponentsShared.Sections;

public partial class Schedule
{
#nullable disable
    [Inject] public IApplicationService ApplicationService { get; set; }
    [Inject] public ISnackbarHelper SnackbarHelper { get; set; }
    [Inject] public IJSRuntime JS { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
#nullable enable

    [Parameter] public List<LoanInstallmentDto>? InstallmentDtos { get; set; }

    protected async override Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }
}