using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MoneyLoaner.ComponentsShared.Sections;
using MoneyLoaner.Data.Forms;
using MoneyLoaner.WebAPI.Services.ApplicationService;
using MudBlazor;

namespace MoneyLoaner.ComponentsShared.Dialogs;

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