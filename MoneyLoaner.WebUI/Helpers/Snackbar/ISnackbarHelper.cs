using MudBlazor;

namespace MoneyLoaner.WebUI.Helpers.Snackbar;

public interface ISnackbarHelper
{
    void Show(string message, Severity s, bool hide = false, bool showDate = true);
}