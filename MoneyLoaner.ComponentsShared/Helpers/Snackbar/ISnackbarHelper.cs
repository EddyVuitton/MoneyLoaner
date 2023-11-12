using MudBlazor;

namespace MoneyLoaner.ComponentsShared.Helpers.Snackbar;

public interface ISnackbarHelper
{
    void Show(string message, Severity s, bool hide = false, bool showDate = true);
}