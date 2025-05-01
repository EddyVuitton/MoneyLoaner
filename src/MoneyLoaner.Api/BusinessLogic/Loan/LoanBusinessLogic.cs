using Dapper;
using Microsoft.Data.SqlClient;
using MoneyLoaner.Domain.DTOs;
using MoneyLoaner.Domain.Helpers;
using MoneyLoaner.Api.Data;
using MoneyLoaner.Api.Extensions;
using System.Collections;
using System.Data;

namespace MoneyLoaner.Api.BusinessLogic.Loan;

public class LoanBusinessLogic(IConfiguration configuration) : ILoanBusinessLogic
{
    private readonly string _connectionString = configuration.GetConnectionString("Database") ?? throw new Exception("Brak connection string do bazy danych");

    #region PublicMethods

    public async Task SubmitNewProposalAsync(NewProposalDto newProposalDto)
    {
        var loan = newProposalDto.LoanDto;
        var proposal = newProposalDto.ProposalDto;

        if (loan is null || proposal is null)
            throw new ArgumentNullException(nameof(newProposalDto));

        ReplaceSpacesToEmptyString(proposal);

        if (loan is null || proposal is null)
            throw new Exception("");

        //przygotuj id klienta
        var customerId = await AddOrGetCustomerAsync(proposal);

        //dodaj rachunek bankowy, który będzie służył do spłacenia pożyczki
        var bankAccountId = await AddNewCCNumberIdAsync();

        //dodaj nową pożyczkę klienta
        var loanId = await AddNewLoanAsync(customerId, bankAccountId);

        //dodaj nowy wniosek
        await AddNewProposalAsync(loan, proposal, loanId);

        //wylicz scoring
        var isDisqualification = await CalculateScoringAsync(loanId);

        if (isDisqualification == 0)
        {
            //dodaj harmonogram pierwotny
            await AddInitialSchedule(loan, loanId);
        }
    }

    public async Task<IEnumerable<LoanInstallmentDto>> GetScheduleAsync(int po_id)
    {
        await using var con = new SqlConnection(_connectionString);

        var param = new { po_id };
        var result = await con.QueryAsync<LoanInstallmentDto>("exec p_pobierz_harmonogram @po_id;", param);

        return result;
    }

    public async Task<AccountInfoDto?> GetAccountInfoAsync(int pk_id)
    {
        await using var con = new SqlConnection(_connectionString);

        var param = new { pk_id };
        var result = await con.QuerySingleAsync<AccountInfoDto>("exec p_konto_informacje_pobierz @pk_id;", param);

        return result;
    }

    public async Task<IEnumerable<LoanHistoryDto>> GetLoansHistoryAsync(int pk_id)
    {
        await using var con = new SqlConnection(_connectionString);

        var param = new { pk_id };
        var result = await con.QueryAsync<LoanHistoryDto>("exec p_konto_historia_pozyczek @pk_id;", param);

        return result;
    }

    public async Task<LoanConfig?> GetLoanConfigAsync()
    {
        await using var con = new SqlConnection(_connectionString);

        var result = await con.QuerySingleAsync<LoanConfig>("exec p_aktualna_oferta_config;");

        return result;
    }

    #endregion PublicMethods

    #region PrivateMethods

    private async Task AddInitialSchedule(LoanDto loan, int loanId)
    {
        await using var con = new SqlConnection(_connectionString);

        var xml = LoanHelper.GenerateXmlT(loan, "raty");
        var param = new
        {
            pd_id = loanId,
            xml
        };
        await con.QueryAsync("exec p_dodaj_harmonogram @pd_id, @xml;", param);
    }

    private async Task<int> AddOrGetCustomerAsync(ProposalDto proposal)
    {
        await using var con = new SqlConnection(_connectionString);

        var param = new { pesel = proposal.PersonalNumber };
        var result = await con.QuerySingleAsync<int>("exec p_klient_pobierz @pesel;", param);

        if (result == 0)
        {
            var param1 = new
            {
                imie = proposal.Name,
                nazwisko = proposal.Surname,
                pesel = proposal.PersonalNumber,
                email = proposal.Email,
                numer_telefonu = proposal.PhoneNumber
            };
            result = await con.QuerySingleAsync<int>("exec p_pozyczka_klient_aktualizuj @imie, @nazwisko, @pesel, @email, @numer_telefonu;", param1);
        }

        return result;
    }

    private async Task<int> AddNewLoanAsync(int customerId, int bankAccountId)
    {
        await using var con = new SqlConnection(_connectionString);

        var param = new
        {
            rb_id = bankAccountId,
            pk_id = customerId
        };
        var result = await con.QuerySingleAsync<int>("exec p_pozyczka_dodaj @rb_id, @pk_id;", param);

        return result;
    }

    private async Task<int> AddNewCCNumberIdAsync()
    {
        await using var con = new SqlConnection(_connectionString);

        var ccNumber = GenerateRandomCCNumber();

        var param = new { numer = ccNumber };
        var result = await con.QuerySingleAsync<int>("exec p_rachunek_bankowy_dodaj @numer;", param);

        return result;
    }

    private async Task<int> AddNewProposalAsync(LoanDto loan, ProposalDto proposal, int loanId)
    {
        await using var con = new SqlConnection(_connectionString);

        var param = new
        {
            pd_id = loanId,
            imie = proposal.Name,
            nazwisko = proposal.Surname,
            numer_telefonu = proposal.PhoneNumber,
            pesel = proposal.PersonalNumber,
            email = proposal.Email,
            miesieczny_dochod = proposal.MonthlyIncome,
            miesieczne_wydatki = proposal.MonthlyExpenses,
            numer_konta = proposal.CCNumber?.Replace(" ", string.Empty)
        };

        var newProposalId = await con.QuerySingleAsync<int>("exec p_pozyczka_wniosek_dodaj @pd_id, @imie, @nazwisko, @numer_telefonu, @pesel, @email, @miesieczny_dochod, @miesieczne_wydatki, @numer_konta;", param);

        var param1 = new
        {
            pwn_id = newProposalId,
            rata_od = loan.InstallmentDtoList!.First().Total,
            data_pierwszej_raty = loan.FirstInstallmentPaymentDate.Date,
            rrso = loan.XIRR,
            okres_splaty = loan.Installments,
            kwota_wnioskowana = loan.Principal,
            prowizja = loan.Fee,
            odsetki = loan.InstallmentDtoList!.Sum(x => x.Interest),
            calkowita_kwota_do_zaplaty = loan.InstallmentDtoList!.Sum(x => x.Total),
            raty_platne_do = loan.DayOfDatePayment
        };

        //dodaj szczegóły oferty
        await con.QueryAsync("exec p_pozyczka_szczegoly_oferty_dodaj @pwn_id, @rata_od, @data_pierwszej_raty, @rrso, @okres_splaty, @kwota_wnioskowana, @prowizja, @odsetki, @calkowita_kwota_do_zaplaty, @raty_platne_do;", param1);

        return newProposalId;
    }

    private static string GenerateRandomCCNumber()
    {
        var random = new Random();
        const string digits = "0123456789";

        var randomChars = new char[24];
        for (int i = 0; i < 24; i++)
        {
            randomChars[i] = digits[random.Next(digits.Length)];
        }

        return "11" + new string(randomChars);
    }

    private static void ReplaceSpacesToEmptyString(ProposalDto proposal)
    {
        proposal.CCNumber = proposal.CCNumber?.Replace(" ", "");
        proposal.PhoneNumber = proposal.PhoneNumber?.Replace(" ", "");
        proposal.Email = proposal.Email?.Replace(" ", "");
    }

    private async Task<int> CalculateScoringAsync(int po_id)
    {
        await using var con = new SqlConnection(_connectionString);

        var param = new {po_id};
        var result = await con.QuerySingleAsync<int>("exec p_scoring_wylicz @po_id;", param);

        return result;
    }

    #endregion PrivateMethods
}