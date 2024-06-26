﻿using Microsoft.AspNetCore.Components;
using MoneyLoaner.Domain.DTOs;
using MoneyLoaner.WebUI.Dialogs;
using MoneyLoaner.WebUI.Helpers.Snackbar;
using MudBlazor;

namespace MoneyLoaner.WebUI.Sections;

public partial class AccountInfo
{
    [Inject] public ISnackbarHelper SnackbarHelper { get; set; } = null!;
    [Inject] public IDialogService DialogService { get; set; } = null!;

    [Parameter] public AccountInfoDto? AccountInfoDto { get; set; }

    protected async override Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        AccountInfoDto ??= new();
    }

    public void FailureAfterSubmitSnackbar(string message)
    {
        SnackbarHelper.Show(message, Severity.Error, false, false);
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

        DialogService.Show<EmailDialog>(null, parameters, options);
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

        DialogService.Show<PhoneDialog>(null, parameters, options);
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

        DialogService.Show<PasswordDialog>(null, parameters, options);
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

    #endregion ChangePassword
}