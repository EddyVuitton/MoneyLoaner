using Microsoft.AspNetCore.Components;

namespace MoneyLoaner.Components.Layout;

public partial class MainLayout
{
    [Inject] public NavigationManager? NavigationManager { get; set; }
    private bool _drawerOpen = true;

    private void NavToIndex()
    {
        NavigationManager!.NavigateTo($"/", true);
        StateHasChanged();
    }
}