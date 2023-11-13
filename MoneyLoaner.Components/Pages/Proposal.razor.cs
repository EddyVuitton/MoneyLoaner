using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MoneyLoaner.ComponentsShared.Extensions;
using MoneyLoaner.Data.DTOs;
using MoneyLoaner.WebAPI.Helpers;
using System.Text.Json;

namespace MoneyLoaner.Components.Pages;

public partial class Proposal
{
    [Inject] public IJSRuntime? JS { get; set; }

    private LoanDto _loan = new();

    protected override async Task OnInitializedAsync()
    {
        if (JS is not null)
        {
            var encryptedJson = await JS.GetFromLocalStorage(EncryptHelper.Encrypt("loan"));
            var decryptedJson = EncryptHelper.Decrypt(encryptedJson.ToString());
            var data = JsonSerializer.Deserialize<LoanDto>(decryptedJson);

            if (data is not null)
            {
                _loan = data;
            }
        }
    }
}