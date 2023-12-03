using Microsoft.AspNetCore.Components;
using MoneyLoaner.WebAPI.Auth;

namespace MoneyLoaner.Components.Layout;

public partial class MainLayout
{
#nullable disable
    [Inject] public ILoginService LoginService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
#nullable enable

    private int _userAccountId = 0;

    protected override async Task OnInitializedAsync()
    {
        var isLoggedIn = await LoginService.IsLoggedInAsync();

        //_userAccountId = isLoggedIn ?? 0;
    }

    private void NavToIndex()
    {
        NavigationManager.NavigateTo($"/", true);
    }
}