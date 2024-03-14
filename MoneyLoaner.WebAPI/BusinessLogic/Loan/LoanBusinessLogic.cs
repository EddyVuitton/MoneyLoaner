using MoneyLoaner.Domain.Context;
using MoneyLoaner.Domain.DTOs;
using MoneyLoaner.WebAPI.Data;
using MoneyLoaner.WebAPI.Extensions;
using MoneyLoaner.Domain.Helpers;
using System.Collections;
using System.Data;
using MoneyLoaner.WebAPI.Helpers;

namespace MoneyLoaner.WebAPI.BusinessLogic.Loan;

public class LoanBusinessLogic : ILoanBusinessLogic
{
    private readonly DBContext _context;

    public LoanBusinessLogic(DBContext context)
    {
        _context = context;
    }

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

    public async Task<List<LoanInstallmentDto>> GetScheduleAsync(int po_id)
    {
        var hT = new object[]
        {
            SqlParam.CreateParameter("po_id", po_id, SqlDbType.Int)
        };

        var result = await _context.SqlQueryAsync<LoanInstallmentDto>("exec p_pobierz_harmonogram @po_id;", hT);

        return result ?? [];
    }

    public async Task<AccountInfoDto?> GetAccountInfoAsync(int pk_id)
    {
        var hT = new object[]
        {
            SqlParam.CreateParameter("pk_id", pk_id, SqlDbType.Int)
        };

        var result = await _context.SqlQueryAsync<AccountInfoDto>("exec p_konto_informacje_pobierz @pk_id;", hT);

        return result?.FirstOrDefault();
    }

    public async Task<List<LoanHistoryDto>?> GetLoansHistoryAsync(int pk_id)
    {
        var hT = new object[]
        {
            SqlParam.CreateParameter("pk_id", pk_id, SqlDbType.Int)
        };

        return await _context.SqlQueryAsync<LoanHistoryDto>("exec p_konto_historia_pozyczek @pk_id;", hT);
    }

    public async Task<LoanConfig?> GetLoanConfigAsync()
    {
        var result = await _context.SqlQueryAsync<LoanConfig>("exec p_aktualna_oferta_config;");

        return result?.FirstOrDefault();
    }
    
    #endregion PublicMethods

    #region PrivateMethods

    private static async Task AddInitialSchedule(LoanDto loan, int loanId)
    {
        var xml = LoanHelper.GenerateXmlT(loan, "raty");
        var hT = new Hashtable
        {
            { "@pd_id", loanId },
            { "@xml", xml }
        };

        await SqlHelper.ExecuteSqlQuerySingleAsync("exec p_dodaj_harmonogram @pd_id, @xml;", hT);
    }

    private static async Task<int> AddOrGetCustomerAsync(ProposalDto proposal)
    {
        var hT = new Hashtable
        {
            { "@pesel", proposal.PersonalNumber }
        };

        var customerInfo = await SqlHelper.ExecuteSqlQuerySingleAsync("exec p_klient_pobierz @pesel;", hT);

        int pk_id;
        if (customerInfo.Count == 0)
        {
            hT = new Hashtable
            {
                { "@imie", proposal.Name },
                { "@nazwisko", proposal.Surname},
                { "@pesel", proposal.PersonalNumber },
                { "@email", proposal.Email },
                { "@numer_telefonu", proposal.PhoneNumber },
                { "@out_id", -1 }
            };

            var newCustomer = await SqlHelper.ExecuteSqlQuerySingleAsync("exec p_pozyczka_klient_aktualizuj @imie, @nazwisko, @pesel, @email, @numer_telefonu, @out_id out;", hT);
            pk_id = int.Parse(newCustomer["@out_id"]!.ToString()!);
        }
        else
        {
            pk_id = int.Parse(customerInfo["pk_id"]!.ToString()!);
        }

        return pk_id;
    }

    private static async Task<int> AddNewLoanAsync(int customerId, int bankAccountId)
    {
        var hT = new Hashtable
        {
            { "@rb_id", bankAccountId},
            { "@pk_id", customerId},
            { "@out_id", -1 }
        };

        var newLoanId = await SqlHelper.ExecuteSqlQuerySingleAsync("exec p_pozyczka_dodaj @rb_id, @pk_id, @out_id out;", hT);

        return int.Parse(newLoanId["@out_id"]!.ToString()!);
    }

    private static async Task<int> AddNewCCNumberIdAsync()
    {
        var ccNumber = GenerateRandomCCNumber();

        var hT = new Hashtable
        {
            { "@numer", ccNumber },
            { "@out_id", -1 }
        };

        var newBankAccountNumber = await SqlHelper.ExecuteSqlQuerySingleAsync("exec p_rachunek_bankowy_dodaj @numer, @out_id out;", hT);

        return int.Parse(newBankAccountNumber["@out_id"]!.ToString()!);
    }

    private static async Task<int> AddNewProposalAsync(LoanDto loan, ProposalDto proposal, int loanId)
    {
        var hT = new Hashtable
        {
            { "@pd_id", loanId },
            { "@imie", proposal.Name },
            { "@nazwisko", proposal.Surname },
            { "@numer_telefonu", proposal.PhoneNumber },
            { "@pesel", proposal.PersonalNumber },
            { "@email", proposal.Email },
            { "@miesieczny_dochod", proposal.MonthlyIncome },
            { "@miesieczne_wydatki", proposal.MonthlyExpenses },
            { "@numer_konta", proposal.CCNumber!.Replace(" ", string.Empty).ToString() },
            { "@out_id", -1 }
        };

        var newProposalId = await SqlHelper.ExecuteSqlQuerySingleAsync("exec p_pozyczka_wniosek_dodaj @pd_id, @imie, @nazwisko, @numer_telefonu, @pesel, @email, @miesieczny_dochod, @miesieczne_wydatki, @numer_konta, @out_id out;", hT);

        hT = new Hashtable
        {
            { "@pwn_id", int.Parse(newProposalId["@out_id"]!.ToString()!)},
            { "@rata_od", loan.InstallmentDtoList!.First().Total },
            { "@data_pierwszej_raty", loan.FirstInstallmentPaymentDate.Date },
            { "@rrso", loan.XIRR },
            { "@okres_splaty", loan.Installments },
            { "@kwota_wnioskowana", loan.Principal },
            { "@prowizja", loan.Fee },
            { "@odsetki", loan.InstallmentDtoList!.Sum(x => x.Interest) },
            { "@calkowita_kwota_do_zaplaty", loan.InstallmentDtoList!.Sum(x => x.Total) },
            { "@raty_platne_do", loan.DayOfDatePayment }
        };

        //dodaj szczegóły oferty
        await SqlHelper.ExecuteSqlQuerySingleAsync("exec p_pozyczka_szczegoly_oferty_dodaj @pwn_id, @rata_od, @data_pierwszej_raty, @rrso, @okres_splaty, @kwota_wnioskowana, @prowizja, @odsetki, @calkowita_kwota_do_zaplaty, @raty_platne_do;", hT);

        return int.Parse(newProposalId["@out_id"]!.ToString()!);
    }

    private static string GenerateRandomCCNumber()
    {
        var random = new Random();
        const string digits = "0123456789";

        char[] randomChars = new char[24];
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

    private static async Task<int> CalculateScoringAsync(int po_id)
    {
        var hT = new Hashtable
        {
            { "@po_id", po_id },
            { "@out_is_disqualification", 0 }
        };

        var isDisqualification = await SqlHelper.ExecuteSqlQuerySingleAsync("exec p_scoring_wylicz @po_id, @out_is_disqualification out;", hT);

        return int.Parse(isDisqualification["@out_is_disqualification"]!.ToString()!);
    }

    #endregion PrivateMethods
}