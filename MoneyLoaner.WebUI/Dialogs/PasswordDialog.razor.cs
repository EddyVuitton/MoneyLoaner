using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MoneyLoaner.Domain.Forms;
using MoneyLoaner.WebUI.Sections;
using MoneyLoaner.WebUI.Services.ApplicationService;
using MudBlazor;

namespace MoneyLoaner.WebUI.Dialogs;

public partial class PasswordDialog
{
#nullable disable
    [Inject] public IApplicationService ApplicationService { get; set; }

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
    [Parameter] public AccountInfo AccountInfoRef { get; set; }
#nullable enable

    private readonly UpdatePasswordForm _model = new();

    private async void OnValidSubmit(EditContext context)
    {
        var result = await ApplicationService.UpdatePasswordAsync(_model);

        if (!result.IsSucces)
        {
            AccountInfoRef.FailureAfterSubmitSnackbar(result.Message!);
            return;
        }

        this.Close();
        AccountInfoRef.AfterChangePasswordSubmit(result.IsSucces);
    }

    private void Close()
    {
        MudDialog.Cancel();
    }
}