using Microsoft.AspNetCore.Components;
using MoneyLoaner.ComponentsShared.Helpers.Snackbar;
using MoneyLoaner.WebAPI.Services.ApplicationService;

namespace MoneyLoaner.Components.Pages;

public partial class Index
{
    [Inject] public IApplicationService? ApplicationService { get; set; }
    [Inject] public ISnackbarHelper? SnackbarHelper { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }
}