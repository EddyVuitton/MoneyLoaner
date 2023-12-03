using Microsoft.AspNetCore.Components;
using MoneyLoaner.ComponentsShared.Helpers.Snackbar;
using MoneyLoaner.WebAPI.Auth;
using MoneyLoaner.WebAPI.Services.ApplicationService;

namespace MoneyLoaner.Components.Pages;

public partial class Index
{
#nullable disable
    [Inject] public IApplicationService ApplicationService { get; set; }
    [Inject] public ISnackbarHelper SnackbarHelper { get; set; }
    [Inject] public ILoginService LoginService { get; set; }
#nullable enable

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }
}