﻿using Microsoft.AspNetCore.Components;
using MoneyLoaner.Domain.DTOs;

namespace MoneyLoaner.WebUI.Sections;

public partial class LoansHistory
{
    [Parameter] public List<LoanHistoryDto>? LoanHistoryDtos { get; set; }
}