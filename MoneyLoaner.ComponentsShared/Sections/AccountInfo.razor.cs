using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MoneyLoaner.ComponentsShared.Dialogs;
using MoneyLoaner.ComponentsShared.Helpers.Snackbar;
using MoneyLoaner.Data.DTOs;
using MoneyLoaner.WebAPI.Services.ApplicationService;
using MudBlazor;

namespace MoneyLoaner.ComponentsShared.Sections;

public partial class AccountInfo
{
#nullable disable
    [Inject] public IApplicationService ApplicationService { get; set; }
    [Inject] public ISnackbarHelper SnackbarHelper { get; set; }
    [Inject] public IJSRuntime JS { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public IDialogService Dialog { get; set; }
#nullable enable

    [Parameter] public AccountInfoDto? AccountInfoDto { get; set; }

    protected async override Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        AccountInfoDto ??= new();
    }

    #region ChangeEmail

    private void DialogChangeEmail()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            NoHeader = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        };

        var parameters = new DialogParameters<EmailDialog>
        {
            { "AccountInfoRef", this }
        };

        Dialog.Show<EmailDialog>(null, parameters, options);
    }

    public void AfterChangeEmailSubmit(bool isSuccess, string email)
    {
        if (isSuccess)
        {
            SnackbarHelper.Show("Pomyślnie zmieniono adres email", Severity.Success, true, false);

            AccountInfoDto!.Email = email;
            StateHasChanged();
        }
        else
        {
            SnackbarHelper.Show("Błąd przy zmianie adresu email", Severity.Warning, true, false);
        }
    }

    #endregion ChangeEmail

    #region ChangePhone

    private void DialogChangePhone()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            NoHeader = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        };

        var parameters = new DialogParameters<PhoneDialog>
        {
            { "AccountInfoRef", this }
        };

        Dialog.Show<PhoneDialog>(null, parameters, options);
    }

    public void AfterChangePhoneSubmit(bool isSuccess, string phone)
    {
        if (isSuccess)
        {
            SnackbarHelper.Show("Pomyślnie zmieniono numer telefonu", Severity.Success, true, false);

            AccountInfoDto!.Phone = phone;
            StateHasChanged();
        }
        else
        {
            SnackbarHelper.Show("Błąd przy zmianie numeru telefonu", Severity.Warning, true, false);
        }
    }

    #endregion ChangePhone

    #region ChangePassword

    private void DialogChangePassword()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            NoHeader = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        };

        var parameters = new DialogParameters<PasswordDialog>
        {
            { "AccountInfoRef", this }
        };

        Dialog.Show<PasswordDialog>(null, parameters, options);
    }

    public void AfterChangePasswordSubmit(bool isSuccess)
    {
        if (isSuccess)
        {
            SnackbarHelper.Show("Pomyślnie zmieniono hasło", Severity.Success, true, false);
        }
        else
        {
            SnackbarHelper.Show("Błąd przy zmianie hasła", Severity.Warning, true, false);
        }
    }

    public void WrongOldPasswordSnackbar(string message)
    {
        SnackbarHelper.Show(message, Severity.Error, false, false);
    }

    #endregion ChangePassword
}