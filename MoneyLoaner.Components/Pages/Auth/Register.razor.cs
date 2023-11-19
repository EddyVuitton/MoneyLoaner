using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MoneyLoaner.Data.DTOs;
using MoneyLoaner.Data.Forms;
using MoneyLoaner.WebAPI.Extensions;
using MoneyLoaner.WebAPI.Helpers;
using System.Text.Json;

namespace MoneyLoaner.Components.Pages.Auth;

public partial class Register
{
    [Inject] public IJSRuntime? JS { get; set; }

    private NewProposalDto _newProposalDto = new();
    private RegisterAccountForm _model = new();

    protected override async Task OnInitializedAsync()
    {
        if (JS is not null)
        {
            var encryptedJson = await JS.GetFromLocalStorage(EncryptHelper.Encrypt("newproposal"));
            var decryptedJson = EncryptHelper.Decrypt(encryptedJson.ToString());
            var data = JsonSerializer.Deserialize<NewProposalDto>(decryptedJson);

            if (data is not null)
            {
                _newProposalDto = data;
                _model.PersonalNumber = _newProposalDto.ProposalDto!.PersonalNumber;
                _model.Email = _newProposalDto.ProposalDto!.Email;
            }
        }
    }

    private void OnValidSubmit(EditContext context)
    {
        StateHasChanged();
    }
}