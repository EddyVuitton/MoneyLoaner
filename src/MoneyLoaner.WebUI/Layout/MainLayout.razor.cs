using Microsoft.AspNetCore.Components;

namespace MoneyLoaner.WebUI.Layout;

public partial class MainLayout
{
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    private readonly int _userAccountId = 0;

    private void NavToIndex()
    {
        NavigationManager.NavigateTo($"/", true);
    }

    private void NavToAccount()
    {
        NavigationManager.NavigateTo($"/account/", true);
    }
}