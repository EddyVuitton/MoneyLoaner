﻿using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using MoneyLoaner.ComponentsShared.Extensions;
using MoneyLoaner.ComponentsShared.Helpers.Snackbar;
using MoneyLoaner.Data.DTOs;
using MoneyLoaner.WebAPI.Data;
using MoneyLoaner.WebAPI.Extensions;
using MoneyLoaner.WebAPI.Helpers;
using MoneyLoaner.WebAPI.Services.ApplicationService;
using System.Text.Json;

namespace MoneyLoaner.ComponentsShared.Sections;

public partial class LoanInfo
{
    [Inject] public IApplicationService? ApplicationService { get; set; }
    [Inject] public ISnackbarHelper? SnackbarHelper { get; set; }
    [Inject] public IJSRuntime? JS { get; set; }
    [Inject] public StateContainer? StateContainer { get; set; }
    [Inject] public NavigationManager? NavigationManager { get; set; }

    private readonly DateTime _now = DateTime.Now;

    private LoanDto _loan = new();
    private List<InstallmentDto> _installmentListDto = new();

    private decimal _loanAmount;
    private decimal _loanAmountMin;
    private decimal _loanAmountMax;
    private int _loanAmountStep;

    private decimal _loanPeriod;
    private decimal _loanPeriodMin;
    private decimal _loanPeriodMax;
    private int _loanPeriodStep;

    private decimal _firstInstallmentTotal;
    private decimal _apr;
    private decimal _fee;
    private decimal _interestRate;
    private DateTime? _DayOfDatePayment;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        await LoadDefulatValues();
    }

    public void NavigateToProposal()
    {
        NavigationManager?.NavigateTo($"/Proposal/");
    }

    private async Task LoadDefulatValues()
    {
        _loanAmount = 5000;
        _loanAmountMin = 1000;
        _loanAmountMax = 25000;
        _loanAmountStep = 100;

        _loanPeriod = 12;
        _loanPeriodMin = 6;
        _loanPeriodMax = 36;
        _loanPeriodStep = 3;

        _fee = 0.1351m;
        _interestRate = 0.1575m;
        _DayOfDatePayment = _now;

        _loan = new LoanDto
        {
            StartDate = _now,
            FirstInstallmentPaymentDate = _now.AddMonths(1),
            DayOfDatePayment = _DayOfDatePayment.Value.Day,
            Installments = Convert.ToInt32(_loanPeriod),
            Principal = _loanAmount,
            Fee = _loanAmount * _fee,
            InterestRate = _interestRate
        };

        _apr = LoanHelper.CalculateXIRR(_loan);
        await CalculateInstallments();
    }

    private async Task LoanValueChanged(decimal value)
    {
        _loanAmount = value;
        _loan!.Principal = _loanAmount;
        _loan!.Fee = _loan!.Principal * _fee;
        await CalculateInstallments();
        StateHasChanged();
    }

    private async Task LoanPeriodValueChanged(decimal value)
    {
        _loanPeriod = value;
        _loan!.Installments = Convert.ToInt32(_loanPeriod);
        await CalculateInstallments();
        StateHasChanged();
    }

    private async Task ChangeDatePayment(DateTime? day)
    {
        if (day is not null)
        {
            _loan.DayOfDatePayment = day.Value.Day;
            await CalculateInstallments();
            StateHasChanged();
        }
    }

    private async Task CalculateInstallments()
    {
        _installmentListDto = LoanHelper.GetInstallmentList(_loan!);
        _firstInstallmentTotal = _installmentListDto.First().Total;
        _loan.InstallmentDtoList = _installmentListDto;

        if (JS is not null)
        {
            var json = JsonSerializer.Serialize(_loan);
            await JS.SetInLocalStorage(EncryptHelper.Encrypt("loan"), EncryptHelper.Encrypt(json));
        }
    }

    private void LoanAmountPlus()
    {
        if (_loanAmount < _loanAmountMax)
        {
            _loanAmount += _loanAmountStep;
        }
    }

    private void LoanAmountMinus()
    {
        if (_loanAmount > _loanAmountMin)
        {
            _loanAmount -= _loanAmountStep;
        }
    }

    private void LoanPeriodPlus()
    {
        if (_loanPeriod < _loanPeriodMax)
        {
            _loanPeriod += _loanPeriodStep;
        }
    }

    private void LoanPeriodMinus()
    {
        if (_loanPeriod > _loanPeriodMin)
        {
            _loanPeriod -= _loanPeriodStep;
        }
    }
}