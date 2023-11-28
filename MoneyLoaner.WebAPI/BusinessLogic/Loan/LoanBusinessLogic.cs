using MoneyLoaner.Data.Context;
using MoneyLoaner.Data.DTOs;
using MoneyLoaner.WebAPI.Helpers;
using System.Collections;

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
            throw new Exception("");

        //przygotuj id klienta
        var customerId = await AddOrGetCustomerAsync(proposal);

        //dodaj rachunek bankowy, na który zostanie wypłacona pożyczka
        var bankAccountId = await AddNewCCNumberIdAsync();

        //dodaj nowy dług klienta
        var loanId = await AddNewLoanAsync(customerId, bankAccountId);

        //dodaj nowy wniosek
        await AddNewProposalAsync(loan, proposal, loanId);

        //dodaj harmonogram pierwotny
        await AddInitialSchedule(loan, loanId);
    }

    #endregion PublicMethods

    #region PrivateMethods

    private static async Task AddInitialSchedule(LoanDto loan, int loanId)
    {
        var xml = LoanHelper.GenerateXmlT(loan.InstallmentDtoList!, "raty");
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

        var newLoanId = await SqlHelper.ExecuteSqlQuerySingleAsync("exec p_pozyczka_dlug_dodaj @rb_id, @pk_id, @out_id out;", hT);

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

    #endregion PrivateMethods
}