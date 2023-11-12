using Microsoft.AspNetCore.Components;
using MoneyLoaner.Data.DTOs;
using MoneyLoaner.WebAPI.Data;
using MoneyLoaner.WebAPI.Extensions;

namespace MoneyLoaner.Components.Pages;

public partial class Proposal
{
    [Inject] public StateContainer? StateContainer { get; set; }

    [Parameter] public int SetHashCode { get; set; }

    private LoanDto _loan = new();

    protected override void OnInitialized()
    {
        if (StateContainer is not null)
        {
            _loan = StateContainer.GetRoutingObjectParameter<LoanDto>(SetHashCode);
        }
    }
}