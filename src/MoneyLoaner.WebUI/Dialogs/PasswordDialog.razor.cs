using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MoneyLoaner.Domain.Forms;
using MoneyLoaner.WebUI.Sections;
using MoneyLoaner.WebUI.Services.ApplicationService;
using MudBlazor;

namespace MoneyLoaner.WebUI.Dialogs;

public partial class PasswordDialog
{
    [Inject] public IApplicationService ApplicationService { get; set; } = null!;

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;

    [Parameter] public AccountInfo? AccountInfoRef { get; set; }

    private readonly UpdatePasswordForm _model = new();

    private async void OnValidSubmit(EditContext context)
    {
        if (AccountInfoRef is null)
        {
            return;
        }

        try
        {
            await ApplicationService.UpdatePasswordAsync(_model);

            Close();
            AccountInfoRef.AfterChangePasswordSubmit();
        }
        catch (Exception ex)
        {
            AccountInfoRef.FailureAfterSubmitSnackbar(ex.Message);
        }
    }

    private void Close()
    {
        MudDialog.Cancel();
    }
}